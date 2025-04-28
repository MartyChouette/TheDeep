// using System.Collections.Generic;
// using UnityEngine;

// public class ObjectTransparencyController : MonoBehaviour
// {
//     public Transform player; // Assign the player object
//     public LayerMask obstacleLayers; // Set which layers can be transparent
//     public float transparencyAmount = 0.3f; // Transparency level when obstructing
//     public float fadeSpeed = 5f; // Speed of transition
//     private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
//     private List<Renderer> currentObstructingObjects = new List<Renderer>();

//     void Update()
//     {
//         HandleTransparency();
//     }

//     void HandleTransparency()
//     {
//         // Reset previous objects
//         foreach (Renderer renderer in currentObstructingObjects)
//         {
//             if (renderer != null)
//             {
//                 RestoreOriginalMaterial(renderer);
//             }
//         }
//         currentObstructingObjects.Clear();

//         // Cast a ray from the camera to the player
//         Vector3 direction = (player.position - transform.position).normalized;
//         float distance = Vector3.Distance(transform.position, player.position);
//         RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, obstacleLayers);

//         foreach (RaycastHit hit in hits)
//         {
//             Renderer renderer = hit.collider.GetComponent<Renderer>();
//             if (renderer != null && !currentObstructingObjects.Contains(renderer))
//             {
//                 ApplyTransparency(renderer);
//                 currentObstructingObjects.Add(renderer);
//             }
           
//         }
//     }

//     void ApplyTransparency(Renderer renderer)
//     {
//         if (!originalMaterials.ContainsKey(renderer))
//         {
//             originalMaterials[renderer] = renderer.materials;
//         }

//         Material[] materials = renderer.materials;
//         foreach (Material mat in materials)
//         {
//             if (mat.HasProperty("_Color"))
//             {
//                 Color color = mat.color;
//                 color.a = transparencyAmount;
//                 mat.color = Color.Lerp(mat.color, color, Time.deltaTime * fadeSpeed);
//                 mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//                 mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//                 mat.SetInt("_ZWrite", 0);
//                 mat.renderQueue = 100;
//             }
//         }
//         Debug.Log("MAKE MATERIAL TRANSPARENT");

//     }

//     void RestoreOriginalMaterial(Renderer renderer)
//     {
//         if (originalMaterials.ContainsKey(renderer))
//         {
//             Material[] materials = renderer.materials;
//             foreach (Material mat in materials)
//             {
//                 if (mat.HasProperty("_Color"))
//                 {
//                     Color color = mat.color;
//                     color.a = 1f;
//                     mat.color = Color.Lerp(mat.color, color, Time.deltaTime * fadeSpeed);
//                     mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
//                     mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
//                     mat.SetInt("_ZWrite", 1);
//                     mat.renderQueue = 1;
//                 }
//             }
//         }
//         Debug.Log("MAKE MATERIAL NONTRANSPARENT");
//     }
// }
