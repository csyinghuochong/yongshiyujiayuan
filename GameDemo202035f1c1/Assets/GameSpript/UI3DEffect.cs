using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UI3DEffect : MonoBehaviour
{
	[SerializeField]
	private GameObject effectPrefab;
	private GameObject effectGO;
	private RenderTexture renderTexture;
	private Camera rtCamera;
	private RawImage rawImage;
    public int EffectNum;           //当前界面第几个特效用于实例化位置，要不UI重叠

	void Awake()
	{
		//RawImage可以手动添加，设置no alpha材质，以显示带透明的粒子
		rawImage = gameObject.GetComponent<RawImage>();
		if (rawImage == null)
		{
			rawImage = gameObject.AddComponent<RawImage>();
		}
	}

	public RectTransform rectTransform
	{
		get
		{
			return transform as RectTransform;
		}
	}

	void OnEnable()
	{
		if (effectPrefab != null)
		{
			effectGO = Instantiate(effectPrefab);

			GameObject cameraObj = new GameObject("UIEffectCamera");
			rtCamera = cameraObj.AddComponent<Camera>();
			renderTexture = new RenderTexture((int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y, 24);
			renderTexture.antiAliasing = 4;
			rtCamera.clearFlags = CameraClearFlags.SolidColor;
			rtCamera.backgroundColor = new Color();
			rtCamera.cullingMask = 1 << 13;
			rtCamera.targetTexture = renderTexture;
			effectGO.transform.SetParent(cameraObj.transform, false);

            //将特效绑定在对应的位置上
            if (Game_PublicClassVar.Get_game_PositionVar.gameObject != null) {
                cameraObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.transform);
            }
            //根据特效顺序排列显示UI
            Vector3 v3 = new Vector3(EffectNum * 100, 0, 0);
            rtCamera.transform.localPosition = v3;
            

			rawImage.enabled = true;
			rawImage.texture = renderTexture;
		}
		else
		{
			rawImage.texture = null;
			Debug.LogError("EffectPrefab can't be null");
		}
	}

	void OnDisable()
	{
		if (effectGO != null)
		{
			Destroy(effectGO);
			effectGO = null;
		}
		if (rtCamera != null)
		{
			Destroy(rtCamera.gameObject);
			rtCamera = null;
		}
		if (renderTexture != null)
		{
			Destroy(renderTexture);
			renderTexture = null;
		}
		rawImage.enabled = false;
	}
}
