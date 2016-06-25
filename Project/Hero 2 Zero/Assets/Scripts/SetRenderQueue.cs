/*
	SetRenderQueue.cs
 
	Sets the RenderQueue of an object's materials on Awake. This will instance
	the materials, so the script won't interfere with other renderers that
	reference the same materials.
*/

using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueue")]

public class SetRenderQueue : MonoBehaviour
{
	
	[SerializeField]
	protected int[] m_queues = new int[]{3000};
	public Material mat;
	public bool changeChildren = true;
	public bool changeSortOrder = false;
	
	protected void Awake()
	{
	
		if (changeChildren) {
			foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
				r.material.renderQueue = m_queues[0];
				
				if (changeSortOrder) {
					r.sortingLayerID = 2;
				}
			}
		}
		
		if (GetComponent<Renderer>() != null) {
			Material[] materials = GetComponent<Renderer>().materials;
	
			for (int i = 0; i < materials.Length && i < m_queues.Length; ++i) {
				materials[i].renderQueue = m_queues[i];
			}		
		}
	}
}