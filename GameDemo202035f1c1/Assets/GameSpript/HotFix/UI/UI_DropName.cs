using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;


//此脚本用于掉落物品的名称显示
public class UI_DropName : MonoBehaviour
{

    public GameObject DropNamePosition;
    public ObscuredString DropItemID;                   //掉落的道具ID
    public ObscuredInt DropItemNum;                     //掉落的道具数据,一般默认为1 像金币或其他货币会用的上
    public ObscuredFloat HideDropPro;                   //极品装备掉落概率
    public ObscuredBool IfRoseTake;                     //道具是否被拾取
    private ObscuredBool DropNameUpdateStatus;          //掉落更新 表示执行一次
    private GameObject obj_dropName;            //掉落物体的UI
    private Vector3 vec3_droName;               //掉落道具名字在UI中的位置
	private ObscuredBool itemDropStatus;				//掉落道具掉落状态

    private ObscuredBool DropMoveStatus;                //掉落的物体向上移动开启
    private ObscuredBool DropMoveDownStatus;            //掉落的物体向下移动开启
    private ObscuredInt moveTime;                       //3D模型移动的次数
    private ObscuredFloat rangeForward;
    private ObscuredFloat rangeRight;
	private Rigidbody dropRigidbody;
	public GameObject dropItemModel;			//3D道具模型
    public MeshRenderer ModelMesh;              //AI材质
    private Rose_Status rose_Status;
    public GameObject EffectDropHightQuality;       //高品质特效

    private Vector3 Move_Target_Position;
    private Vector3 OnceDropPosition;
    private ObscuredBool StopMoveStatus;

    private ObscuredFloat DropUpTime;
    private ObscuredBool DropUpStatus;

    private ObscuredFloat dropTimeSum;

    private string itemType;
    private string itemQuality;
    private bool canShiQuStatus;

    //private float dis_1;

    //public GameObject DropBox;      //碰撞大小

    //private Rigidboy aa;
    // Use this for initialization
    void Start () {

        //初始化数据
        DropNameUpdateStatus = true;
        DropMoveStatus = true;
        moveTime = 0;
		//设置刚体属性
		dropRigidbody = GetComponent<Rigidbody>();
        if (dropRigidbody != null) {
            dropRigidbody.useGravity = false;
        }
        //随机一个物体移动的值
        rangeForward = Random.Range(0f, 0.3f) - 0.15f;
        rangeRight = Random.Range(0f, 0.3f) - 0.15f;
		if (rangeForward < 0.03f) {
			rangeForward = rangeForward + 0.03f;
		}
		if (rangeRight < 0.03f) {
			rangeRight = rangeRight + 0.03f;
		}
		//Debug.Log (this.gameObject.name + "随机值为：" + rangeForward.ToString () + "," + rangeRight.ToString ());

		this.GetComponent<BoxCollider>().isTrigger = true;

		//rangeForward = 0f;
		//rangeRight = 0f;



		//显示道具模型
		//dropItemModel = (GameObject)MonoBehaviour.Instantiate(Resources.Load("3DModel/Item/wuqi"));
        //显示道具Icon
        string dropItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", DropItemID, "Item_Template");

        //金币特殊处理
        if (DropItemID == "1") {
            dropItemIcon = "DropGold";
        }

        object obj = Resources.Load("ItemIcon/" + dropItemIcon, typeof(Texture2D));
        Texture2D itemIcon = obj as Texture2D;
        //UI_ItemIcon.GetComponent<Image>().sprite = itemIcon;
        ModelMesh.material.mainTexture = itemIcon;
        //ModelMesh.material = 

        dropItemModel.transform.SetParent(this.gameObject.transform);
		dropItemModel.transform.localPosition = new Vector3(0,0,0);
		dropItemModel.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
		dropItemModel.transform.localScale = new Vector3(0.075f, 1.0f, 0.075f);

        //初始化碰撞体为false
        this.GetComponent<SphereCollider>().enabled = false;

        rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        //获取第一次的位置
        OnceDropPosition = this.gameObject.transform.position;

        //判定当前道具是否是高品质装备
        itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", DropItemID, "Item_Template");
        itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", DropItemID, "Item_Template");
        if (itemQuality == "" || itemQuality == null) {
            itemQuality = "1";
        }


        if (itemType == "3") {
            if (int.Parse(itemQuality)>=4) {
                EffectDropHightQuality.SetActive(true);
            }
        }
        if (int.Parse(itemQuality) >= 5)
        {
            EffectDropHightQuality.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {

        DropUpTime = DropUpTime + Time.deltaTime;

		//Debug.Log (this.gameObject.transform.localRotation);
        //判定与角色相距30米进行名称显示
        float distance = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, this.gameObject.transform.position);
        float AI_Distance = Vector3.Distance(rose_Status.gameObject.transform.position, this.gameObject.transform.position);
        if (distance <= 30.0f)
        {
            canShiQuStatus = true;
            //挂机状态
            if (rose_Status.AutomaticGuaJiStatus) {

                //这里可以处理默认拾取那些道具 ,挂机默认品质下的不拾取
                /*
                if (int.Parse(itemQuality) < 3) {
                    canShiQuStatus = false;
                }
                */

                if (Game_PublicClassVar.Get_game_PositionVar.GuaJiShiQuType_1 != "0") {
                    //材料
                    if (itemType == "2") {
                        canShiQuStatus = false;
                    }
                }

                if (Game_PublicClassVar.Get_game_PositionVar.GuaJiShiQuType_2 != "0")
                {
                    //装备
                    if (itemType == "3")
                    {
                        canShiQuStatus = false;
                    }
                }

            }
            if (canShiQuStatus) {
                //设置掉落自动拾取
                //Rose_Status rose_status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                float roseDis = rose_Status.NextAutomaticDropDis;
                if (rose_Status.NextAutomaticDropObj == null)
                {
                    rose_Status.NextAutomaticDropObj = this.gameObject;
                    rose_Status.NextAutomaticDropDis = AI_Distance;
                }
                if (rose_Status.NextAutomaticDropObj == this.gameObject)
                {
                    rose_Status.NextAutomaticDropDis = AI_Distance;
                }
                if (AI_Distance < roseDis)
                {
                    rose_Status.NextAutomaticDropObj = this.gameObject;
                    rose_Status.NextAutomaticDropDis = AI_Distance;
                }
            }

            //修正物体在屏幕中的位置
			vec3_droName = Camera.main.WorldToViewportPoint(DropNamePosition.transform.position);
			vec3_droName = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_droName);
		
            //第一次显示
            if (DropNameUpdateStatus == true && Game_PublicClassVar.Get_game_PositionVar.Obj_UIDropName != null)
            {
                DropNameUpdateStatus = false;
                //实例化UI
                obj_dropName = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIDropName);
                if (obj_dropName != null)
                {
                    obj_dropName.SetActive(false);
                    obj_dropName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_DropItemSet.transform);
                    obj_dropName.transform.localPosition = new Vector3(vec3_droName.x, vec3_droName.y, 0);
                    obj_dropName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                //显示道具名称
                string dropItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", DropItemID,"Item_Template");
                string dropItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", DropItemID, "Item_Template");
                GameObject obj_LabDropName = obj_dropName.transform.Find("Lab_DropName").gameObject;
                if (obj_LabDropName != null)
                {
                    obj_LabDropName.transform.GetComponent<TMP_Text>().text = dropItemName;
                    if (DropItemID == "1")
                    {
                        obj_LabDropName.transform.GetComponent<TMP_Text>().text = DropItemNum.ToString() + dropItemName;
                        dropItemQuality = "2";
                    }
                }

                //显示道具品质颜色
                obj_LabDropName.GetComponent<TMP_Text>().color = Game_PublicClassVar.Get_function_UI.QualityReturnColor(dropItemQuality);
            }
            //修正掉落物体的位置
            if (obj_dropName != null) {
                obj_dropName.transform.localPosition = new Vector3(vec3_droName.x, vec3_droName.y, 0);
            }


			if (itemDropStatus) {
				if (obj_dropName != null) {
					obj_dropName.SetActive(true);
				}
			}
        }
        else {
            //不显示掉落名称（清理不需要显示的内容防止卡）
            DropNameUpdateStatus = true;

            if (obj_dropName != null)
            {
                MonoBehaviour.Destroy(obj_dropName);
            }
            
        }

        //出现物体时向上移动（3D模型）
        if (DropMoveStatus == true) {

			this.transform.localPosition = new Vector3(this.transform.localPosition.x+rangeForward,this.transform.localPosition.y + 0.5f - 0.02f * moveTime * -2,this.transform.localPosition.z+rangeRight);
            moveTime = moveTime + 1;
			dropItemModel.transform.localRotation = Quaternion.Euler(new Vector3 (18*moveTime, 0, 0));

            //向上移动10次
            if (moveTime >= 5) {
                DropMoveStatus = false;
                DropMoveDownStatus = true;
                moveTime = 0;
                if (dropRigidbody != null) {
                    dropRigidbody.useGravity = true;	//开启刚体
                }
				
				dropItemModel.transform.localRotation = Quaternion.Euler(new Vector3 (180, 0, 0));

                //发送一条射线
                //Debug.Log("OnceDropPosition = " + OnceDropPosition);
                Ray Move_Target_Ray = Camera.main.ScreenPointToRay(OnceDropPosition);
                                    //声明一个光线触碰器
                    RaycastHit Move_Target_Hit;
                    LayerMask mask = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 12) | (1 << 13);

                    //检测射线是否碰触到对象
                    //标记OUT变量在传入参数进去后可能发生改变，和ref类似，但是ref需要给他一个初始值
                    //第一个参数射线变量  第二个参数光线触碰器的反馈变量
                    if (Physics.Raycast(Move_Target_Ray, out Move_Target_Hit, 100, mask.value))
                    {
                        //获取碰撞地面
                        if (Move_Target_Hit.collider.gameObject.layer == 10)
                        {
                            if (Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus == false)
                            {
                                //将碰触的三维坐标进行赋值
                                Move_Target_Position = Move_Target_Hit.point;

                            }
                        }
                    }
            }
        }
        //出现的物体开始向下移动（3D模型）
        if (DropMoveDownStatus == true) {
			if (dropRigidbody != null) {
				dropRigidbody.velocity = new Vector3(0,-moveTime*2,0);
				dropRigidbody.drag = 0;
				this.transform.localPosition = new Vector3(this.transform.localPosition.x+rangeForward/2,this.transform.localPosition.y,this.transform.localPosition.z+rangeRight/2);
				dropItemModel.transform.localRotation = Quaternion.Euler(new Vector3 (180, 0, 0));
				this.GetComponent<BoxCollider> ().isTrigger = false;
			}

            moveTime = moveTime + 1;

            //向下移动25次后停止移动
            if (moveTime >= 25)
            {
                DropMoveDownStatus = false;
                moveTime = 0;
                this.GetComponent<SphereCollider>().enabled = true; //移动结束 开启拾取
            }
            //if(Move_Target_Position.y<=)
            if (this.transform.position.y <= OnceDropPosition.y)
            {
                DropMoveDownStatus = false; //停止移动
                stopMove();
                this.transform.position = new Vector3(this.transform.position.x, OnceDropPosition.y + 0.2f, this.transform.position.z);
            }
        }

        //当金币距离玩家只有5米时,金币自动被拾取
        if(DropItemID=="1"){
            //获取角色是否为移动状态
            if (rose_Status.RoseStatus == "2") {
                float dis = Vector3.Distance(this.transform.position, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                if (dis <= 0.5f)
                {
                    Game_PublicClassVar.Get_function_Rose.SendReward("1", DropItemNum.ToString(),"29");
                    IfRoseTake = true;
                }
            }
        }
        //如果道具被拾取，注销对应的3D模型和UI
        if (IfRoseTake) {

            //播放音效
            if (DropItemID == "1")
            {
                Game_PublicClassVar.Get_function_UI.PlaySource("20012", "2");
            }
            else {
                Game_PublicClassVar.Get_function_UI.PlaySource("20009", "2");
            }

            if (obj_dropName != null)
            {
                MonoBehaviour.Destroy(obj_dropName);              //销毁UI
            }
            
            MonoBehaviour.Destroy(this.gameObject);           //销毁3D模型

			//获取道具名称
			string dropItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", DropItemID,"Item_Template");
			UI_RoseGetItemHint hint = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseGetItemHint.GetComponent<UI_RoseGetItemHint>();
			hint.UpdataHintText = "拾取"+dropItemName+"X"+DropItemNum;
			hint.UpdataHint = true;

        }

        if (!StopMoveStatus) {
            //如果掉落道具距离自己太远 也会停止移动,防止道具掉下
            if (this.transform.position.y <= OnceDropPosition.y)
            {
                DropMoveDownStatus = false; //停止移动
                stopMove();
                this.transform.position = new Vector3(this.transform.position.x, OnceDropPosition.y + 0.2f, this.transform.position.z);
            }
            StopMoveStatus = true;
        }


        //大于2秒监测道具拾取不到的问题
        if (!DropUpStatus) {
            if (DropUpTime >= 2.0f)
            {
                //如果掉落道具距离自己太远 也会停止移动,防止道具掉下
                if (this.transform.position.y - OnceDropPosition.y > 2.0f)
                {
                    DropMoveDownStatus = false; //停止移动
                    stopMove();
                    this.transform.position = new Vector3(this.transform.position.x, OnceDropPosition.y + 0.2f, this.transform.position.z);
                }
            }
            DropUpStatus = true;
        }


        dropTimeSum = dropTimeSum + Time.deltaTime;
        if (dropTimeSum >= 1800) {
            Destroy(this.gameObject);

            if (obj_dropName != null)
            {
                MonoBehaviour.Destroy(obj_dropName);              //销毁UI
            }
           
        }


    }

    //OnCollisionEnter
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("碰撞到" + collision.collider.gameObject.name);
		if (collision.collider.gameObject.layer == 10) {
			if (!DropMoveStatus) {
                if (dropRigidbody != null) {
                    Destroy(dropRigidbody);
                }

                if (obj_dropName != null)
                {
                    obj_dropName.SetActive(true);
                }
				
				this.GetComponent<BoxCollider>().isTrigger = false;
				dropItemModel.transform.localRotation = Quaternion.Euler(new Vector3 (180, 0, 0));
				itemDropStatus = true;
                //dis_1 = 
			}
			//
		}
	}

    //射线方式停止掉落
    void stopMove() {
        if (dropRigidbody != null)
        {
            Destroy(dropRigidbody);
        }
        if (obj_dropName != null)
        {
            obj_dropName.SetActive(true);
        }
        this.GetComponent<BoxCollider>().isTrigger = false;
        dropItemModel.transform.localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
        itemDropStatus = true;
    }
}
