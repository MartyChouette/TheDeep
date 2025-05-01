using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiCollider))]
public class SimpleRopeClimber : MonoBehaviour
{
    [Header("Climb settings")]
    public float climbSpeed = 2f;          // metres per second along rope
    public float handYOffset = 0f;         // tweak so hands line up with rope

    ObiCollider obiCol;
    ObiRope rope;                      // rope currently inside trigger
    ObiPinConstraintsData pinData;
    ObiPinConstraintsBatch batch;

    bool climbing;
    int elemIndex;                       // index in rope.elements
    float segPos;                          // metres from particle2 toward particle1

    /* -------------------------------------------------- */
    void Awake()
    {
        obiCol = GetComponent<ObiCollider>();
        batch = new ObiPinConstraintsBatch();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!climbing && rope != null) StartClimb();
            else if (climbing) StopClimb();
        }

        if (climbing)
            MoveAlongRope(Input.GetAxisRaw("Vertical")); // W/S
    }

    /* trigger picks rope --------------------------------------------------- */
    void OnTriggerEnter(Collider c) { if (c.TryGetComponent(out ObiRope r)) rope = r; }
    void OnTriggerExit(Collider c) { if (c.TryGetComponent(out ObiRope r) && r == rope) { rope = null; StopClimb(); } }

    /* climb control -------------------------------------------------------- */
    void StartClimb()
    {
        climbing = true;

        pinData = rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiPinConstraintsData;
        pinData.Clear();
        batch.Clear();

        elemIndex = NearestElement(transform.position);
        segPos = 0f;

        int solverIndex = rope.solverIndices[rope.elements[elemIndex].particle2]; // lower particle
        batch.AddConstraint(solverIndex, obiCol, Vector3.zero, Quaternion.identity, 0f, float.PositiveInfinity);
        batch.activeConstraintCount = 1;
        pinData.AddBatch(batch);
        rope.SetConstraintsDirty(Oni.ConstraintType.Pin);

        UpdatePin();
    }

    void StopClimb()
    {
        if (!climbing) return;
        climbing = false;

        pinData?.Clear();
        rope?.SetConstraintsDirty(Oni.ConstraintType.Pin);
    }

    /* move up/down --------------------------------------------------------- */
    void MoveAlongRope(float axis)          // axis = +1 (W)   or -1 (S)
    {
        if (Mathf.Abs(axis) < 0.01f) return;

        float step = axis * climbSpeed * Time.deltaTime;
        segPos += step;

        /* climb up (toward rope start) */
        while (segPos > rope.elements[elemIndex].restLength && elemIndex > 0)
        {
            segPos -= rope.elements[elemIndex].restLength;
            elemIndex--;
        }

        /* climb down (toward rope end) */
        while (segPos < 0 && elemIndex < rope.elements.Count - 1)
        {
            elemIndex++;
            segPos += rope.elements[elemIndex].restLength;
        }

        /* clamp at rope ends */
        segPos = Mathf.Clamp(segPos, 0, rope.elements[elemIndex].restLength);

        UpdatePin();
    }

    /* update pin offset ---------------------------------------------------- */
    void UpdatePin()
    {
        var elem = rope.elements[elemIndex];

        Vector3 p1 = rope.solver.positions[elem.particle1]; // upper
        Vector3 p2 = rope.solver.positions[elem.particle2]; // lower

        float t = elem.restLength > 0 ? segPos / elem.restLength : 0f;
        Vector3 worldHand = Vector3.Lerp(p2, p1, t) + Vector3.up * handYOffset;

        batch.offsets[0] = obiCol.transform.InverseTransformPoint(worldHand);
        rope.SetConstraintsDirty(Oni.ConstraintType.Pin);

        transform.rotation = Quaternion.LookRotation(p1 - p2, Vector3.up);
    }

    /* helper --------------------------------------------------------------- */
    int NearestElement(Vector3 worldPos)
    {
        int best = 0; float min = float.MaxValue;

        for (int i = 0; i < rope.elements.Count; ++i)
        {
            float y = rope.solver.positions[rope.elements[i].particle2].y;
            float d = Mathf.Abs(worldPos.y - y);
            if (d < min) { min = d; best = i; }
        }
        return best;
    }
}
