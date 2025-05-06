using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiCollider))]
public class ObiRopeClimber : MonoBehaviour
{
    [Header("Climb settings")]
    public float climbSpeed = 2f;         
    public float handYOffset = 0f;         

    ObiCollider obiCol;
    ObiRope rope;
    ObiPinConstraintsData pins;
    ObiPinConstraintsBatch batch;

    CharacterController cc;   
    Rigidbody rb;   

    public bool climbing;
    int particleSolverIndex;   
    float worldY;                

    
    void Awake()
    {
        obiCol = GetComponent<ObiCollider>();
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
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
            SlideAlongRope(Input.GetAxisRaw("Vertical")); 
    }

    
    void OnTriggerEnter(Collider c) { if (c.TryGetComponent(out ObiRope r)) rope = r; }
    void OnTriggerExit(Collider c) { if (c.TryGetComponent(out ObiRope r) && r == rope) { rope = null; StopClimb(); } }

   
    void StartClimb()
    {
        if (rope == null) return;
        climbing = true;

        if (cc) cc.enabled = false;
        if (rb) { rb.isKinematic = true; rb.freezeRotation = true; }

        
        pins = rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiPinConstraintsData;
        pins.Clear();
        batch.Clear();
        batch.AddConstraint(0, obiCol, Vector3.zero, Quaternion.identity,
                            0f, float.PositiveInfinity);
        batch.activeConstraintCount = 1;
        pins.AddBatch(batch);

        
        int elem = NearestElement(transform.position);
        particleSolverIndex = rope.solverIndices[rope.elements[elem].particle2];
        batch.particleIndices[0] = particleSolverIndex;

        worldY = rope.solver.positions[particleSolverIndex].y + handYOffset;
        UpdatePinOffset();
    }

    void StopClimb()
    {
        if (!climbing) return;
        climbing = false;

        pins?.Clear();
        rope?.SetConstraintsDirty(Oni.ConstraintType.Pin);

        if (cc) cc.enabled = true;
        if (rb) { rb.isKinematic = false; rb.freezeRotation = false; }
    }

    
    void SlideAlongRope(float axis)   
    {
        if (Mathf.Abs(axis) < 0.01f) return;

        worldY += axis * climbSpeed * Time.deltaTime;

        
        float bottom = rope.solver.positions[rope.solverIndices[rope.elements[^1].particle2]].y;
        float top = rope.solver.positions[rope.solverIndices[rope.elements[0].particle1]].y;
        worldY = Mathf.Clamp(worldY, bottom, top);

        UpdatePinOffset();
    }

    
    void UpdatePinOffset()
    {
        Vector3 worldPos = rope.solver.positions[particleSolverIndex];
        worldPos.y = worldY;

        
        batch.offsets[0] = obiCol.transform.InverseTransformPoint(worldPos);
        rope.SetConstraintsDirty(Oni.ConstraintType.Pin);

        
        Vector3 up = transform.up;
        transform.rotation = Quaternion.identity * Quaternion.AngleAxis(transform.eulerAngles.y, up);
    }

   
    int NearestElement(Vector3 pos)
    {
        int best = 0;
        float min = float.MaxValue;

        for (int i = 0; i < rope.elements.Count; i++)
        {
            Vector3 p = rope.solver.positions[rope.elements[i].particle2];
            float d = Mathf.Abs(pos.y - p.y);
            if (d < min) { min = d; best = i; }
        }
        return best;
    }
}
