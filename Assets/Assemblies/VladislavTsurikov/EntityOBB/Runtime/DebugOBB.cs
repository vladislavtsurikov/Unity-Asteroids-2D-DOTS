using UnityEngine;

namespace VladislavTsurikov.EntityOBB.Runtime
{
    /// Add this component to the scene, then the code will know whether to display the OBB.
    /// This is an unusual solution, but during development there was a problem displaying Gizmos.DrawWireCube in System,
    /// because Gizmos.DrawWireCube cannot be called in Update, but must be called in the OnDrawGizmos() event method
    public class DebugOBB : MonoBehaviour
    {
        
    }
}