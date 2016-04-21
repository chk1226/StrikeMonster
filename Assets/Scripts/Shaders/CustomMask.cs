using UnityEngine;
using System.Collections;


namespace MyShader
{

	[ExecuteInEditMode]
	[RequireComponent(typeof(UnityEngine.UI.Image))]
	public class CustomMask : MonoBehaviour
	{

		private UnityEngine.UI.Image m_CustomMask = null;
		public UnityEngine.UI.Image Mask
		{
			get
			{
				if (m_CustomMask == null)
				{
					m_CustomMask = GetComponent<UnityEngine.UI.Image>();
				}
				return m_CustomMask;
			}
		}

		public Vector2 Center = Vector2.zero;
		public Vector2 Radius = Vector2.one * 0.1f;
		public float BaseAlpha = 0.5f;
		public bool AlwaysUpdate = false;
		public bool AutoCalculateRadius = false;
		public Color AddColor = new Color(0f, 0f, 0f);
		public float Offset = 0.05f;
		public GameObject Target;

		public enum MaskType
		{
			Free,
			ReferenceObject
		}
		public MaskType Type = MaskType.Free;

		private static Material m_Material = null;
		protected Material material
		{
			get
			{
				if (m_Material == null)
				{
					Shader shader = null;
					if ((shader = Shader.Find("Custom/CustomMask")) != null)
					{
						m_Material = new Material(shader);
						m_Material.hideFlags = HideFlags.DontSave;
					}
				}
				return m_Material;
			}
		}
		private static readonly Vector2 MIN_RADIUS = new Vector2(0.0001f, 0.0001f);

		// Use this for initialization
		void Start()
		{
//			m_CustomMask = GetComponent<UnityEngine.UI.Image>();
			Mask.type = UnityEngine.UI.Image.Type.Simple;
			if (material == null || !material.shader.isSupported)
			{
				gameObject.SetActive(false);
				return;
			}
			else
			{
				Mask.material = material;
			}

		}

		// Update is called once per frame
		void Update()
		{
			if (AlwaysUpdate)
			{
				Render();
			}
		}

		public void Render()
		{
			if (material != null)
			{
				if (Type == MaskType.Free)
				{
					Radius = Vector2.Min(Vector2.one, Vector2.Max(Radius, MIN_RADIUS));
					Center = Vector2.Min(Vector2.one, Vector2.Max(Center, Vector2.zero));

				}
				else if (Type == MaskType.ReferenceObject)
				{
					if (Target == null)
					{
						Debug.LogWarning("Gameobject Target is null.");
						return;
					}

					var reg = Mask.rectTransform.rect;
					var scale_wh = Mask.rectTransform.TransformVector(reg.width, reg.height, 0);
					Rect mask = new Rect(Mask.transform.position.x - scale_wh.x / 2,
										 Mask.transform.position.y - scale_wh.y / 2,
										 scale_wh.x,
										 scale_wh.y);


					Center.x = -1;
					Center.y = -1;

					var t_rect = Target.GetComponent<RectTransform>();
					if (t_rect != null)
					{
						Vector3[] points = new Vector3[4];
						t_rect.GetWorldCorners(points);
						var target_wp = points[0] + ((points[1] - points[0]) / 2.0f) + ((points[3] - points[0]) / 2.0f);

						if (mask.Contains(new Vector2(target_wp.x, target_wp.y)))
						{
							var offset = new Vector2(target_wp.x, target_wp.y) - new Vector2(mask.x, mask.y);
							Center.x = offset.x / mask.width;
							Center.y = offset.y / mask.height;
						}
					}

					if (AutoCalculateRadius)
					{
						if (t_rect != null)
						{
//							if(Center.x >= 0 || Center.y >= 0)
//							{
//								scale_wh = t_rect.TransformVector(t_rect.rect.width, t_rect.rect.height, 0);
//								var length = 0.5f * Vector2.Distance(new Vector2(target_wp.x, target_wp.y),
//								                                     new Vector2(target_wp.x + scale_wh.x, target_wp.y + scale_wh.y));
//								Radius.x = length / mask.width;
//								Radius.y = length / mask.height;
//								Debug.LogWarning(length.ToString());
//							}
							scale_wh = t_rect.TransformVector(t_rect.rect.width, t_rect.rect.height, 0);
							Radius.x = scale_wh.x / (mask.width * 2) + Offset;
							Radius.y = scale_wh.y / (mask.height * 2) + Offset;
						}
						else
						{
							Debug.LogWarning("Target no have RectTransform component.");
						}
					}

					Radius = Vector2.Min(Vector2.one, Vector2.Max(Radius, MIN_RADIUS));

				}

				material.SetVector("_Radius", Radius);
				material.SetVector("_Center", Center);
				material.SetFloat("_BaseAlpha", BaseAlpha);
				material.SetColor("_AddColor", AddColor);

#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
			}
		}

	}


}