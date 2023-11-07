#region copyright
// ------------------------------------------------------------------------
//  Copyright (C) 2013-2019 Dmitriy Yukhanov - focus [http://codestage.net]
// ------------------------------------------------------------------------
#endregion

#if (UNITY_WINRT || UNITY_WINRT_10_0 || UNITY_WSA || UNITY_WSA_10_0) && !ENABLE_IL2CPP
#define ACTK_UWP_NO_IL2CPP
#endif

namespace CodeStage.AntiCheat.ObscuredTypes
{
	using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;
	using Utils;

    /// <summary>
    /// Use it instead of regular <c>int</c> for any cheating-sensitive variables.
    /// </summary>
    /// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong>
    [Serializable]
    public struct ObscuredInt : IFormattable, IEquatable<ObscuredInt>, IComparable<ObscuredInt>, IComparable<int>, IComparable
    {

		class CheckObInt
		{
			private string _checkint;

			public string CheckInt {

				get { return _checkint; }
				set { _checkint = value; }

			}

			public CheckObInt()
			{
				CheckInt = cryptoKey111.ToString();
			}
		}

        struct IntRandAddKey
        {

            public int KeyValue
            {
                get
                {
                    byte[] buffer = Guid.NewGuid().ToByteArray();
                    int iSeed = BitConverter.ToInt32(buffer, 0);
                    System.Random aaa = new System.Random(iSeed);
                    int returnValue = aaa.Next(10000, 99999);
                    return returnValue;
                }
            }
        };

        private static IntRandAddKey cryptoKey111;
        [SerializeField]
        private static int cryptoKey;

		public static Action<string> sendError;

        //System.Random rand = new System.Random();
        //private static int cryptoKey = DateTime.Now.Millisecond;
#if UNITY_EDITOR
        // For internal Editor usage only (may be useful for drawers).
        public static int cryptoKeyEditor = cryptoKey;
#endif

        [SerializeField]
        private int currentCryptoKey;


		[SerializeField] //加密前的结果
		private int fakeValue;  

        [SerializeField]
        private string hiddenValueStr;      //加密字符串

        [SerializeField]
        private string fakeValueStr;        //解密字符串

		private CheckObInt testCheck;

		[SerializeField]
		private bool fakeValueActive;

		[SerializeField]
        private string hiddenValueAddStr;      //加密字符串 再加密

		[SerializeField]
		private bool inited;

		[SerializeField]
		private bool fakeValueActiveEx;     //自定义效验参数

		private string checkInit;

		private CheckObInt checkObInt;

		[SerializeField]
		private int hiddenValue;            //加密后的结果

		//[SerializeField]
		//private static string hiddenValueAddKey;      //加密字符串 再加密

		private ObscuredInt(int value)
        {
            //初始化
            cryptoKey = cryptoKey111.KeyValue;
            currentCryptoKey = cryptoKey;
            fakeValueStr = "";

            hiddenValue = Encrypt(value);           //加密字
            hiddenValueStr = hiddenValue.ToString();

            hiddenValueAddStr = "";
			//hiddenValueAddStr = KeyEncrypt(hiddenValueStr, currentCryptoKey.ToString());
			checkInit = currentCryptoKey.ToString();

			checkObInt = new CheckObInt();
			testCheck =  new CheckObInt();
#if UNITY_EDITOR
			fakeValue = value + currentCryptoKey;    //存储值
			fakeValueActive = true;
			fakeValueActiveEx = true;
#else
            /*
			var detectorRunning = Detectors.ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = detectorRunning ? value : 0;
			fakeValueActive = detectorRunning;
			fakeValue = fakeValue + currentCryptoKey;
            */

            var detectorRunning = Detectors.ObscuredCheatingDetector.ExistsAndIsRunning;
            fakeValue = detectorRunning ? value : value;
			fakeValue = fakeValue + currentCryptoKey;
            fakeValueActive = detectorRunning;
			fakeValueActiveEx = detectorRunning;
#endif

			//fakeValueStr = fakeValue.ToString();
			inited = true;
		}

        /*
        public static void IntAddHideKey() {

            if (hiddenValueAddKey == "" || hiddenValueAddKey == null) {
                string encryptKey = "";

                if (cryptoKey.ToString().Length >= 2)
                {
                    encryptKey = "/" + cryptoKey.ToString().Substring(0, 2) + "}";
                }

                hiddenValueAddKey = encryptKey;
            }

        }
        */

		/// <summary>
		/// Allows to change default crypto key of this type instances. All new instances will use specified key.<br/>
		/// All current instances will use previous key unless you call ApplyNewCryptoKey() on them explicitly.
		/// </summary>
		public static void SetNewCryptoKey(int newKey)
		{
			cryptoKey = newKey;
            Debug.LogError("设置了新的key"+ newKey);
		}

		/// <summary>
		/// Simple symmetric encryption, uses default crypto key.
		/// </summary>
		/// <returns>Encrypted <c>int</c>.</returns>
		public static int Encrypt(int value)
		{
			return Encrypt(value, 0);
		}

        //加密算法
		/// <summary>
		/// Simple symmetric encryption, uses passed crypto key.
		/// </summary>
		/// <returns>Encrypted <c>int</c>.</returns>
		public static int Encrypt(int value, int key)
		{
            //Debug.Log("加密key = " + cryptoKey);
            int returnInit = 0;
            if (key == 0)
            {
                /*
                IntRandAddKey nowRandAddKey;
                int addKey = nowRandAddKey.KeyValue;
                cryptoKey = addKey;
                currentCryptoKey = addKey;
                cryptoKeyEditor = addKey;
                */
                returnInit = value ^ cryptoKey;
				//returnInit = returnInit << 1;
				//Debug.Log("value = " + value + "cryptoKey = " + cryptoKey  + "returnInit = " + returnInit + "oldValue = " + oldValue);
				//oldValue = returnInit;
				//return value ^ cryptoKey;
			}
            else {
                returnInit = value ^ key;
				//returnInit = returnInit << 1;
				//returnInit = value ^ cryptoKey;
			}

            //hiddenValueStr = returnInit.ToString();
            return returnInit ;
            //return value ^ key;
            //return value ^ cryptoKey;
        }



		/// <summary>
		/// Simple symmetric encryption, uses default crypto key.
		/// </summary>
		/// <returns>Decrypted <c>int</c>.</returns>
		public static int Decrypt(int value)
		{
			return Decrypt(value, 0);
		}


        //解密算法
		/// <summary>
		/// Simple symmetric encryption, uses passed crypto key.
		/// </summary>
		/// <returns>Decrypted <c>int</c>.</returns>
		public static int Decrypt(int value, int key)
		{
            //Debug.Log("解密key = " + cryptoKey);
            int returnInit = 0;
            if (key == 0)
            {
				//value = value >> 1;
				returnInit = value ^ cryptoKey;
				//return value ^ cryptoKey;
			}
            else {
				//value = value >> 1;
				returnInit = value ^ key;
				//returnInit = value ^ cryptoKey;
			}

            //return value ^ key;
            //fakeValueStr = returnInit.ToString();

            return returnInit;
        }

		/// <summary>
		/// Creates and fills obscured variable with raw encrypted value previously got from GetEncrypted().
		/// </summary>
		/// Literally does same job as SetEncrypted() but makes new instance instead of filling existing one,
		/// making it easier to initialize new variables from saved encrypted values.
		///
		/// Make sure this obscured type currently has same crypto key as when encrypted value was got with GetEncrypted().
		/// It will be same (default) if you did not used SetNewCryptoKey().
		/// <param name="encrypted">Raw encrypted value you got from GetEncrypted().</param>
		/// <returns>New obscured variable initialized from specified encrypted value.</returns>
		/// \sa GetEncrypted(), SetEncrypted()
		public static ObscuredInt FromEncrypted(int encrypted)
		{
			var instance = new ObscuredInt();
			instance.SetEncrypted(encrypted);
			return instance;
		}

		/// <summary>
		/// Use it after SetNewCryptoKey() to re-encrypt current instance using new crypto key.
		/// </summary>
		public void ApplyNewCryptoKey()
		{
			if (currentCryptoKey != cryptoKey)
			{
				hiddenValue = Encrypt(InternalDecrypt(), cryptoKey);
				currentCryptoKey = cryptoKey;
            }
		}

		/// <summary>
		/// Allows to change current crypto key to the new random value and re-encrypt variable using it.
		/// Use it for extra protection against 'unknown value' search.
		/// Just call it sometimes when your variable doesn't change to fool the cheater.
		/// </summary>
		public void RandomizeCryptoKey()
		{
            hiddenValue = InternalDecrypt();
			currentCryptoKey = ThreadSafeRandom.Next();
			hiddenValue = Encrypt(hiddenValue, currentCryptoKey);
		}

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		public int GetEncrypted()
		{
			ApplyNewCryptoKey();
			return hiddenValue;
		}

		/// <summary>
		/// Allows to explicitly set current obscured value. Crypto key should be same as when encrypted value was got with GetEncrypted().
		/// </summary>
		/// Use it in conjunction with GetEncrypted().<br/>
		/// Useful for loading data stored in obscured state.
		/// \sa FromEncrypted()
		public void SetEncrypted(int encrypted)
		{
			inited = true;
			hiddenValue = encrypted;

			if (currentCryptoKey == 0)
			{
				currentCryptoKey = cryptoKey;
            }

			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				fakeValueActive = false;
				fakeValue = InternalDecrypt() + currentCryptoKey;
				fakeValueActive = true;
			}
			else
			{
				fakeValueActive = false;
			}
		}

        //读取数据
        /// <summary>
        /// Alternative to the type cast, use if you wish to get decrypted value 
        /// but can't or don't want to use cast to the regular type.
        /// </summary>
        /// <returns>Decrypted value.</returns>
        public int GetDecrypted()
		{
			return InternalDecrypt();
		}

		//读取数据
		private int InternalDecrypt()
		{
			if (currentCryptoKey != 0 && hiddenValueStr != null && hiddenValueStr != "")
			{
				fakeValueActive = true;
				fakeValueActiveEx = true;
			}

            if (inited == false && checkInit == null && checkObInt == null )
			{
				//如果没有加载过此值将返回类型的默认值
				currentCryptoKey = cryptoKey;
				hiddenValue = Encrypt(0);
				hiddenValueStr = hiddenValue.ToString();
				//hiddenValueAddStr = KeyEncrypt(hiddenValueStr, currentCryptoKey.ToString());
				fakeValue = 0 + currentCryptoKey;
				fakeValueActive = false;
				fakeValueActiveEx = false;
				inited = true;
				checkInit = currentCryptoKey.ToString();
				checkObInt = new CheckObInt();
				return 0;
			}

            var decrypted = Decrypt(hiddenValue, currentCryptoKey);     //计算出解密的数字
            fakeValueStr = decrypted.ToString();

            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != (fakeValue - currentCryptoKey) )
			{
				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
                //Debug.LogError("数据验证失败,存在修改内容" + "读取值 hiddenValue = " + hiddenValue + "currentCryptoKey = " + currentCryptoKey + "decrypted = " + decrypted + "fakeValue = " + fakeValue);
                //Application.Quit();     //强制退出
            }

            bool ifSendStatus = true;
			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActiveEx && hiddenValueStr != hiddenValue.ToString() && hiddenValueStr != "" && hiddenValueStr != null) {
                ifSendStatus = false;
				//Debug.LogError("数据验证失败" + "hiddenValueStr = " + hiddenValueStr + "hiddenValue = " + hiddenValue);
            }

            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActiveEx && fakeValueStr != (fakeValue - currentCryptoKey).ToString() && fakeValueStr != "" && fakeValueStr != null)
            {
                ifSendStatus = false;
            }

			if (currentCryptoKey < 10000 && checkInit != null && checkObInt != null)
			{
				//Debug.LogError("currentCryptoKey < 10000");
				//Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
				ifSendStatus = false;
			}

			/*          取消检测 会卡,后续需要要优化
			string coststr = KeyDecrypt(hiddenValueAddStr, currentCryptoKey.ToString());
            if (coststr != hiddenValueStr)
            {
                ifSendStatus = false;
                //Debug.LogError("数据2次验证失败" + "hiddenValueStr = " + hiddenValueStr + ";coststr = " + coststr + ";fakeValueStr = " + fakeValueStr + ";cryptoKey = " + cryptoKey);
            }
            else {
                if (coststr != null && coststr != "")
                {
                    int costValue = Decrypt(int.Parse(coststr), currentCryptoKey);
                    if (costValue != decrypted)
                    {
                        ifSendStatus = false;
                        Debug.LogError("数据3次验证失败" + "hiddenValueStr = " + hiddenValueStr + ";coststr = " + coststr + ";fakeValueStr = " + fakeValueStr + ";cryptoKey = " + cryptoKey);
                    }
                }
                else {
                    if (decrypted != 0) {
                        ifSendStatus = false;
                    }
                }
            }
            */

			if (ifSendStatus)
            {
                return decrypted;
            }
            else {
				
				//Debug.LogError("Init数据验证失败,存在修改内容" + "读取值 hiddenValue = " + hiddenValue + "currentCryptoKey = " + currentCryptoKey + "decrypted = " + decrypted + "fakeValue = " + fakeValue);
				//Application.Quit();     //强制退出
				Debug.LogError("数据验证失败,存在修改内容..");
				PlayerPrefs.SetString("RootStatusError", "1");
				if (sendError != null)
				{
					sendError("Init数据验证失败,存在修改内容" + "读取值 hiddenValue = " + hiddenValue + "currentCryptoKey = " + currentCryptoKey + "decrypted = " + decrypted + "fakeValue = " + fakeValue);
				}

				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();

				return 0;
            }

            return 1;
        }

#region operators, overrides, interface implementations
		//! @cond
		public static implicit operator ObscuredInt(int value)
		{
			return new ObscuredInt(value);
		}

		public static implicit operator int(ObscuredInt value)
		{
			return value.InternalDecrypt();
		}

		public static implicit operator ObscuredFloat(ObscuredInt value)
		{
			return value.InternalDecrypt();
		}

		public static implicit operator ObscuredDouble(ObscuredInt value)
		{
			return value.InternalDecrypt();
		}

		public static explicit operator ObscuredUInt(ObscuredInt value)
		{
			return (uint)value.InternalDecrypt();
		}

		public static ObscuredInt operator ++(ObscuredInt input)
		{
			var decrypted = input.InternalDecrypt() + 1;

            input.hiddenValue = Encrypt(decrypted, input.currentCryptoKey);

            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				input.fakeValue = decrypted + input.currentCryptoKey;
				input.fakeValueActive = true;
			}
			else
			{
				input.fakeValueActive = false;
			}

			return input;
		}

		public static ObscuredInt operator --(ObscuredInt input)
		{
			var decrypted = input.InternalDecrypt() - 1;

            input.hiddenValue = Encrypt(decrypted, input.currentCryptoKey);


            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				input.fakeValue = decrypted + input.currentCryptoKey;
				input.fakeValueActive = true;
			}
			else
			{
				input.fakeValueActive = false;
			}

			return input;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// 
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation.
		/// </summary>
		/// 
		/// <returns>
		/// The string representation of the value of this instance, consisting of a negative sign if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeros.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return InternalDecrypt().ToString();
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation, using the specified format.
		/// </summary>
		/// 
		/// <returns>
		/// The string representation of the value of this instance as specified by <paramref name="format"/>.
		/// </returns>
		/// <param name="format">A numeric format string (see Remarks).</param><exception cref="T:System.FormatException"><paramref name="format"/> is invalid or not supported. </exception><filterpriority>1</filterpriority>

		public string ToString(string format)
		{
			return InternalDecrypt().ToString(format);
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.
		/// </summary>
		/// 
		/// <returns>
		/// The string representation of the value of this instance as specified by <paramref name="provider"/>.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information. </param><filterpriority>1</filterpriority>
		public string ToString(IFormatProvider provider)
		{
			return InternalDecrypt().ToString(provider);
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.
		/// </summary>
		/// 
		/// <returns>
		/// The string representation of the value of this instance as specified by <paramref name="format"/> and <paramref name="provider"/>.
		/// </returns>
		/// <param name="format">A numeric format string (see Remarks).</param><param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information. </param><exception cref="T:System.FormatException"><paramref name="format"/> is invalid or not supported.</exception><filterpriority>1</filterpriority>
		public string ToString(string format, IFormatProvider provider)
		{
			return InternalDecrypt().ToString(format, provider);
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// 
		/// <returns>
		/// true if <paramref name="obj"/> is an instance of ObscuredInt and equals the value of this instance; otherwise, false.
		/// </returns>
		/// <param name="obj">An object to compare with this instance. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (!(obj is ObscuredInt))
				return false;
			return Equals((ObscuredInt)obj);
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified ObscuredInt value.
		/// </summary>
		/// 
		/// <returns>
		/// true if <paramref name="obj"/> has the same value as this instance; otherwise, false.
		/// </returns>
		/// <param name="obj">An ObscuredInt value to compare to this instance.</param><filterpriority>2</filterpriority>
		public bool Equals(ObscuredInt obj)
		{
			if (currentCryptoKey == obj.currentCryptoKey)
			{
				return hiddenValue == obj.hiddenValue;
			}

			return Decrypt(hiddenValue, currentCryptoKey) == Decrypt(obj.hiddenValue, obj.currentCryptoKey);
		}

		public int CompareTo(ObscuredInt other)
		{
			return InternalDecrypt().CompareTo(other.InternalDecrypt());
		}

		public int CompareTo(int other)
		{
			return InternalDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
#if !ACTK_UWP_NO_IL2CPP
			return InternalDecrypt().CompareTo(obj);
#else
			if (obj == null) return 1;
			if (!(obj is int)) throw new ArgumentException("Argument must be int");
			return CompareTo((int)obj);
#endif
		}

		//! @endcond
		#endregion

		//加密字符串
		public static string KeyEncrypt(string str, string addkey)
		{

			try
			{

                //空字符串无需加密
                if (str == ""|| str == null) {
                    return str;
                }

                string encryptKey = "/92}";
                /*
                Debug.Log("encryptKey000 = " + encryptKey);
                if (cryptoKey.ToString().Length >= 2)
                {
                    encryptKey = "/" + cryptoKey.ToString().Substring(0, 2) + "}";
                }
                */

                /*
                if (hiddenValueAddKey == ""|| hiddenValueAddKey == null) {
                    IntAddHideKey();
                }

                string encryptKey = hiddenValueAddKey;
                */

                //Debug.Log("加密encryptKey111 = " + encryptKey + ";cryptoKey = " + cryptoKey);
                //Debug.Log("加密前str = " + str + ";cryptoKey = " + cryptoKey);

                byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密钥
				byte[] data = Encoding.Unicode.GetBytes(str);//待加密字符串

				DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密对象
				MemoryStream MStream = new MemoryStream();//内存流对象

				//用内存流实例化加密流对象
				CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
				CStream.Write(data, 0, data.Length);//向加密流中写入数据
				CStream.FlushFinalBlock();//将数据压入基础流
				byte[] temp = MStream.ToArray();//从内存流中获取字节序列
				CStream.Close();//关闭加密流
				MStream.Close();//关闭内存流

                string addStr = Convert.ToBase64String(temp);//返回加密后的字符串
                addStr = addStr + addkey;
                //Debug.Log("加密后str = " + addStr);
                return addStr;

            }
			catch(Exception ex)
            {
                Debug.LogError("加密错误:" + ex);
                return str;
			}
		}

		//解密字符串
		public static string KeyDecrypt(string str,string costkey)
		{

			try
			{
                //空字符串无需加密
                if (str == "" || str == null)
                {
                    return str;
                }

                str = str.Replace(costkey, "");  

                /*
                if (hiddenValueAddKey == "" || hiddenValueAddKey == null)
                {
                    IntAddHideKey();
                }
                string encryptKey = hiddenValueAddKey;
                */

                string encryptKey = "/92}";
                /*
                Debug.Log("encryptKey000 = " + encryptKey);
                if (cryptoKey.ToString().Length >= 2)
                {
                    encryptKey = "/" + cryptoKey.ToString().Substring(0, 2) + "}";
                }
                */

                //Debug.Log("解密encryptKey111 = " + encryptKey + ";cryptoKey = " + cryptoKey);
                //Debug.Log("解密str = " + str + ";cryptoKey = " + cryptoKey);

                byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密钥
				byte[] data = Convert.FromBase64String(str);//待解密字符串

				DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密对象
				MemoryStream MStream = new MemoryStream();//内存流对象

				//用内存流实例化解密流对象
				CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
				CStream.Write(data, 0, data.Length);//向加密流中写入数据
				CStream.FlushFinalBlock();//将数据压入基础流
				byte[] temp = MStream.ToArray();//从内存流中获取字节序列
				CStream.Close();//关闭加密流
				MStream.Close();//关闭内存流
                //Debug.Log("解密后str = " + Encoding.Unicode.GetString(temp));
                string addStr = Encoding.Unicode.GetString(temp);//返回解密后的字符串
                return addStr;

            }
			catch(Exception ex)
			{
                Debug.LogError("解密错误:" + ex);
				return str;
			}
		}

	}

}