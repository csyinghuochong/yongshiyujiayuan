using UnityEngine;
using System.Collections;
using UnityEditor;

public class ApplyMaterial : EditorWindow
{
	
  public static  string  [] config =
	{
		".PNG",".JPG",".TGA"
	};

   [MenuItem ("Window/ApplyMaterial")]	
    static void Applay ()
	{       
	
		Rect  wr = new Rect (0,0,500,500);
        ApplyMaterial window = (ApplyMaterial)EditorWindow.GetWindowWithRect (typeof (ApplyMaterial),wr,true,"widow name");	
		window.Show();
		
		
		

    }
	
	void OnGUI ()
	{
			if(GUILayout.Button("批量关联材质"))
			{
				ApplayMatrials(true);
			}
			if(GUILayout.Button("批量删除关联"))
			{
				ApplayMatrials(false);
			}
            if (GUILayout.Button("替换材质")) {
                TiHuanCaiZhi();
            }
            if (GUILayout.Button("替换材质文件")) {
                TiHuanCaiZhiFile();
            }
	}
	
	
	 void ApplayMatrials(bool isAdd)
	{
        //判定选中物体是否为空
		if(Selection.activeGameObject != null)
		{
            //逐个获取选中的物体
			foreach(GameObject g in Selection.gameObjects)
			{
                //获取每个渲染物体的的子路径
				Renderer []renders = g.GetComponentsInChildren<Renderer>();
                //循环获取自物体的渲染器
				foreach(Renderer r in renders)
				{
                    //如果渲染器不为空
					if(r  !=  null)
					{
                        //循环获得材质
						foreach(Object o in r.sharedMaterials)
						{
                                //获得材质的文件路径
								string path = AssetDatabase.GetAssetPath(o);
                                //根据路径加载一个新的材质
                                Material m = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
                                if (m != null) {
                                    string texturePath = AssetDatabase.GetAssetPath(m.mainTexture);
                                    Debug.Log("texturePath = " + texturePath);
                                }
                                
                                //判定是否添加材质
								if(isAdd)
								{
                                    //添加材质,如果材质不为空
									if(m.mainTexture  == null)
									{
										Texture t = GetTexture(m.name);
										if(t != null)
										{
											m.mainTexture = t;
										}else
										{
											Debug.Log("材质名:" + o.name + " 材质替换失败，请检查资源" );
										}
									}
								}else
								{
									m.mainTexture = null;
								
								}
						}
					}
				}
			}	
			
			this.ShowNotification(new GUIContent("批量关联材质贴图成功"));
		}else
		{
				this.ShowNotification(new GUIContent("没有选择游戏对象"));
		}
	}

     void TiHuanCaiZhiFile() { 
              //判定选中物体是否为空
         if (Selection.activeObject != null)
         {
             //逐个获取选中的物体
             foreach (Object o in Selection.objects)
             {
                 //获得材质的文件路径
                 string path = AssetDatabase.GetAssetPath(o);
                 //根据路径加载一个新的材质
                 Material m = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
                 if (m != null)
                 {
                     string texturePath = AssetDatabase.GetAssetPath(m.mainTexture);
                     texturePath = texturePath.Substring(0, texturePath.LastIndexOf("/") + 1);

                     Texture t = GetTextureStr(m.name, texturePath,"png");
                     if (t != null)
                     {
                         m.mainTexture = t;
                     }
                     else
                     {
                         Debug.Log("材质替换失败! 贴图地址：" + AssetDatabase.GetAssetPath(m.mainTexture));
                     }
                 }
             }
         }
     }


     void TiHuanCaiZhi()
     {
         //判定选中物体是否为空
         if (Selection.activeGameObject != null)
         {
             //逐个获取选中的物体
             foreach (GameObject g in Selection.gameObjects)
             {
                 //获取每个渲染物体的的子路径
                 Renderer[] renders = g.GetComponentsInChildren<Renderer>();
                 //循环获取自物体的渲染器
                 foreach (Renderer r in renders)
                 {
                     //如果渲染器不为空
                     if (r != null)
                     {
                         //循环获得材质
                         foreach (Object o in r.sharedMaterials)
                         {
                             //获得材质的文件路径
                             string path = AssetDatabase.GetAssetPath(o);
                             //根据路径加载一个新的材质
                             Material m = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
                             if (m != null)
                             {
                                 string texturePath = AssetDatabase.GetAssetPath(m.mainTexture);
                                 texturePath = texturePath.Substring(0, texturePath.LastIndexOf("/")+1);
                                 //Debug.Log("texturePath = " + texturePath);

                                 Texture t = GetTextureStr(m.name,texturePath,"jpg");
                                 if (t != null)
                                 {
                                     m.mainTexture = t;
                                 }
                                 else
                                 {
                                     t = GetTextureStr(m.name, texturePath, "png");
                                     if (t != null)
                                     {
                                         m.mainTexture = t;
                                     }
                                     else {
                                         //Debug.Log("材质名:" + o.name + " 材质替换失败，请检查资源");
                                         Debug.Log("材质替换失败! 贴图地址：" + AssetDatabase.GetAssetPath(m.mainTexture));
                                     }

                                 }
                             }


                            //添加材质,如果材质不为空
                             /*
                            if (m.mainTexture == null)
                            {
                                Texture t = GetTexture(m.name);
                                if (t != null)
                                {
                                    m.mainTexture = t;
                                }
                                else
                                {
                                    Debug.Log("材质名:" + o.name + " 材质替换失败，请检查资源");
                                }
                            }
                              */
                         }
                     }
                 }
             }

             this.ShowNotification(new GUIContent("批量关联材质贴图成功"));
         }
         else
         {
             this.ShowNotification(new GUIContent("没有选择游戏对象"));
         }
     }


	
	static Texture GetTexture(string name)
	{
		foreach(string suffix in config)
		{
            Debug.Log("name = " + name + "suffix = " + suffix);
            string png = ".png";
            Texture t = AssetDatabase.LoadAssetAtPath("Assets/Textures/" + name + "png" + png, typeof(Texture)) as Texture;
            if (t != null)
            {
                return t;
            }
            else {
                Debug.Log("找不到指定资源路径："+"Assets/Textures/" + name + "png" + png);
            }
				
		}
			return null;						
	}

    static Texture GetTextureStr(string name,string filePath,string png)
    {
        foreach (string suffix in config)
        {
            //Debug.Log("name = " + name + "suffix = " + suffix);
            //string png = ".png";
            Texture t = AssetDatabase.LoadAssetAtPath(filePath + name + "_png" + png, typeof(Texture)) as Texture;
            if (t != null)
            {
                return t;
            }
            else
            {
                Debug.Log("找不到指定资源路径：" + "Assets/Textures/" + name + "png" + png);
            }

        }
        return null;
    }
}
