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
	using Common;

	using System;
	using UnityEngine;
	using System.Runtime.InteropServices;
	using UnityEngine.Serialization;
	using Utils;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Use it instead of regular <c>float</c> for any cheating-sensitive variables.
    /// </summary>
    /// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong>
    [Serializable]
	public struct ObscuredFloat : IFormattable, IEquatable<ObscuredFloat>, IComparable<ObscuredFloat>, IComparable<float>, IComparable
	{

		class CheckObFloat
		{
			private string _checkfloat;

			public string CheckFloat
			{

				get { return _checkfloat; }
				set { _checkfloat = value; }

			}

			public CheckObFloat()
			{
				CheckFloat = cryptoKey111.ToString();
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

#if UNITY_EDITOR
		// For internal Editor usage only (may be useful for drawers).
		public static int cryptoKeyEditor = cryptoKey;
		public string migratedVersion;
#endif

		[SerializeField]
		[FormerlySerializedAs("hiddenValue")]
#pragma warning disable 414
		private ACTkByte4 hiddenValueOldByte4;
#pragma warning restore 414

		private CheckObFloat checkObTest;

		[SerializeField]
		private int fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		[SerializeField]
        private string hiddenValueStr;      //加密字符串

        [SerializeField]
        private string fakeValueStr;        //解密字符串

        [SerializeField]
        private string hiddenValueAddStr;      //加密字符串 再加密

		[SerializeField]
		private bool inited;

		[SerializeField]
		private bool fakeValueActiveEx;     //自定义效验参数

		private string checkInit;

		[SerializeField]
		private int currentCryptoKey;

		[SerializeField]
		private int hiddenValue;

		private CheckObFloat checkObFloat;

		private ObscuredFloat(float value)
		{

            //初始化
            cryptoKey = cryptoKey111.KeyValue;
            currentCryptoKey = cryptoKey;
            hiddenValueStr = "";
            fakeValueStr = "";

            //currentCryptoKey = cryptoKey;
            hiddenValue = InternalEncrypt(value);
            hiddenValueStr = hiddenValue.ToString();
            hiddenValueAddStr = "";
            //hiddenValueAddStr = KeyEncrypt(hiddenValueStr, currentCryptoKey.ToString());

            hiddenValueOldByte4 = default(ACTkByte4);

			checkInit = currentCryptoKey.ToString();
			checkObFloat = new CheckObFloat();
			checkObTest = new CheckObFloat();
#if UNITY_EDITOR
			fakeValue =  Encrypt(value, 100);
		    fakeValueActive = true;
			migratedVersion = null;
			fakeValueActiveEx = true;
#else
            /*
			var detectorRunning = Detectors.ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = Encrypt((detectorRunning ? value : 0f), 100);
			fakeValueActive = detectorRunning;
            */

            var detectorRunning = Detectors.ObscuredCheatingDetector.ExistsAndIsRunning;
            fakeValue = Encrypt( (detectorRunning ? value : value) , 100);
            fakeValueActive = detectorRunning;
			fakeValueActiveEx= detectorRunning;
#endif

			inited = true;
		}

		/// <summary>
		/// Allows to change default crypto key of this type instances. All new instances will use specified key.<br/>
		/// All current instances will use previous key unless you call ApplyNewCryptoKey() on them explicitly.
		/// </summary>
		public static void SetNewCryptoKey(int newKey)
		{
			cryptoKey = newKey;
		}

		/// <summary>
		/// Use this simple encryption method to encrypt any float value, uses default crypto key.
		/// </summary>
		public static int Encrypt(float value)
		{
			return Encrypt(value, cryptoKey);
		}

		/// <summary>
		/// Use this simple encryption method to encrypt any float value, uses passed crypto key.
		/// </summary>
		/// Make sure you're using key at least of 1000000000 value to improve security.
		public static int Encrypt(float value, int key)
		{
			var u = new FloatIntBytesUnion {f = value};
			u.i = u.i ^ key;
			u.b4.Shuffle();
			return u.i;
		}

		private static int InternalEncrypt(float value, int key = 0)
		{
			var currentKey = key;
			if (currentKey == 0)
			{
				currentKey = cryptoKey;
			}

			return Encrypt(value, currentKey);
		}

		/// <summary>
		/// Use it to decrypt int you got from Encrypt(float) back to float, uses default crypto key.
		/// </summary>
		public static float Decrypt(int value)
		{
			return Decrypt(value, cryptoKey);
		}

		/// <summary>
		/// Use it to decrypt int you got from Encrypt(float) back to float, uses passed crypto key.
		/// </summary>
		/// Make sure you're using key at least of 1000000000 value to improve security.
		public static float Decrypt(int value, int key)
		{
			var u = new FloatIntBytesUnion {i = value};
			u.b4.UnShuffle();
			u.i ^= key;
			return u.f;
		}


		/// <summary>
		/// Allows to update the raw encrypted value to the newer encryption format.
		/// </summary>
		/// Use when you have some encrypted values saved somewhere with previous ACTk version
		/// and you wish to set them using SetEncrypted() to the newer ACTk version obscured type.
		/// Current migration variants:
		/// from 0 or 1 to 2 - migrate obscured type from ACTk 1.5.2.0-1.5.8.0 to the 1.5.9.0+ format
		/// <param name="encrypted">Encrypted value you got from previous ACTk version obscured type with GetEncrypted().</param>
		/// <param name="fromVersion">Source format version.</param>
		/// <param name="toVersion">Target format version.</param>
		/// <returns>Migrated raw encrypted value which you may use for SetEncrypted(0 later.</returns>
		public static int MigrateEncrypted(int encrypted, byte fromVersion = 0, byte toVersion = 2)
		{
			var u = new FloatIntBytesUnion {i = encrypted};

			if (fromVersion < 2 && toVersion == 2)
			{
				u.b4.Shuffle();
			}

			return u.i;
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
		public static ObscuredFloat FromEncrypted(int encrypted)
		{
			var instance = new ObscuredFloat();
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
				hiddenValue = InternalEncrypt(InternalDecrypt(), cryptoKey);
				currentCryptoKey = cryptoKey;
                Debug.Log("ApplyNewCryptoKey");
            }
		}

		/// <summary>
		/// Allows to change current crypto key to the new random value and re-encrypt variable using it.
		/// Use it for extra protection against 'unknown value' search.
		/// Just call it sometimes when your variable doesn't change to fool the cheater.
		/// </summary>
		public void RandomizeCryptoKey()
		{
			var decrypted = InternalDecrypt();
			currentCryptoKey = ThreadSafeRandom.Next(100000, 999999);
			hiddenValue = InternalEncrypt(decrypted, currentCryptoKey);
            Debug.Log("RandomizeCryptoKey");
		}

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		public int GetEncrypted()
		{
			ApplyNewCryptoKey();
            Debug.Log("GetEncrypted");
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
            Debug.Log("SetEncrypted");
            if (currentCryptoKey == 0)
			{
				currentCryptoKey = cryptoKey;
			}

			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				fakeValueActive = false;
				fakeValue = Encrypt(InternalDecrypt(), 100);
				fakeValueActive = true;
			}
			else
			{
				fakeValueActive = false;
			}
		}

		/// <summary>
		/// Alternative to the type cast, use if you wish to get decrypted value 
		/// but can't or don't want to use cast to the regular type.
		/// </summary>
		/// <returns>Decrypted value.</returns>
		public float GetDecrypted()
		{
			return InternalDecrypt();
		}

		private float InternalDecrypt()
		{
			if (currentCryptoKey != 0 && hiddenValueStr != null && hiddenValueStr != "")
			{
				fakeValueActive = true;
				fakeValueActiveEx = true;
			}

			if (inited == false && checkInit == null && checkObFloat == null )
			{
				currentCryptoKey = cryptoKey;
				hiddenValue = InternalEncrypt(0);
                hiddenValueStr = hiddenValue.ToString();
                fakeValue = Encrypt(0f, 100);
				fakeValueActive = false;
				fakeValueActiveEx = false;
				inited = true;
				checkInit = currentCryptoKey.ToString();
				checkObFloat = new CheckObFloat();

				return 0;
			}

#if ACTK_OBSCURED_AUTO_MIGRATION
			if (hiddenValueOldByte4.b1 != 0 || 
			    hiddenValueOldByte4.b2 != 0 || 
				hiddenValueOldByte4.b3 != 0 || 
				hiddenValueOldByte4.b4 != 0)
			{
				var union = new FloatIntBytesUnion {b4 = hiddenValueOldByte4};
				union.b4.Shuffle();
				hiddenValue = union.i;

				hiddenValueOldByte4.b1 = 0;
				hiddenValueOldByte4.b2 = 0;
				hiddenValueOldByte4.b3 = 0;
				hiddenValueOldByte4.b4 = 0;
			}
#endif

			var decrypted = Decrypt(hiddenValue, currentCryptoKey);
            fakeValueStr = decrypted.ToString();


			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && Math.Abs(decrypted - Decrypt(fakeValue, 100)) > Detectors.ObscuredCheatingDetector.Instance.floatEpsilon)
			{
				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}


            bool ifSendStatus = true;

			/* 此处加入验证会报错
            if (hiddenValueStr != hiddenValue.ToString() && hiddenValueStr != "" && hiddenValueStr != null)
            {
                ifSendStatus = false;
                Debug.LogError("float数据验证失败" + "hiddenValueStr = " + hiddenValueStr + "hiddenValue = " + hiddenValue);
            }
            */
				


			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActiveEx && fakeValueStr != (Decrypt(fakeValue, 100)).ToString() && fakeValueStr != "" && fakeValueStr != null)
            {
				ifSendStatus = false;
                //Debug.LogError("float2222数据验证失败" + "hiddenValueStr = " + hiddenValueStr + "hiddenValue = " + hiddenValue + ";fakeValueStr = " + fakeValueStr);
                Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
            }
			
			/*
            string coststr = KeyDecrypt(hiddenValueAddStr, currentCryptoKey.ToString());
            if (coststr != hiddenValueStr)
            {
                ifSendStatus = false;
                //Debug.LogError("数据2次验证失败" + "hiddenValueStr = " + hiddenValueStr + ";coststr = " + coststr + ";fakeValueStr = " + fakeValueStr + ";cryptoKey = " + cryptoKey);
            }
            else
            {
                if (coststr != null && coststr != "")
                {
                    float costValue = Decrypt(int.Parse(coststr), currentCryptoKey);
                    if (costValue != decrypted)
                    {
                        ifSendStatus = false;
                        Debug.LogError("数据3次验证失败" + "hiddenValueStr = " + hiddenValueStr + ";coststr = " + coststr + ";fakeValueStr = " + fakeValueStr + ";cryptoKey = " + cryptoKey);
                    }
                }
                else
                {
                    if (decrypted != 0)
                    {
                        ifSendStatus = false;
                    }
                }
            }
            */
			//Debug.Log("读取数据....decrypted = " + decrypted);

			if (currentCryptoKey < 10000 && checkObFloat != null && checkInit != null)
			{
				/*
				Debug.LogError("Float数据验证失败,currentCryptoKey < 10000" + "读取值 hiddenValue = " + hiddenValue + "currentCryptoKey = " + currentCryptoKey + "decrypted = " + decrypted + "fakeValue = " + fakeValue + " checkInit = " + checkInit);

				if (checkObFloat == null)
				{
					Debug.LogError("checkObFloat为空");
				}
				else {
					Debug.LogError("checkObFloat不为空");
				}
				*/

				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}

			if (ifSendStatus)
            {
                return decrypted;
            }
            else
            {
				//Debug.LogError("Float数据验证失败,存在修改内容" + "读取值 hiddenValue = " + hiddenValue + "currentCryptoKey = " + currentCryptoKey + "decrypted = " + decrypted + "fakeValue = " + fakeValue);
				Debug.LogError("数据验证失败,存在修改内容..");

				PlayerPrefs.SetString("RootStatusError", "1");
				if (sendError != null)
				{
					sendError("Init数据验证失败,存在修改内容" + "读取值 hiddenValue = " + hiddenValue + "currentCryptoKey = " + currentCryptoKey + "decrypted = " + decrypted + "fakeValue = " + fakeValue);
				}

				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
				//Application.Quit();     //强制退出
				return 0;
            }

            return 1;

            //return decrypted;
		}

#region operators, overrides, interface implementations

		//! @cond
		public static implicit operator ObscuredFloat(float value)
		{
			return new ObscuredFloat(value);
		}

		public static implicit operator float(ObscuredFloat value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredFloat operator ++(ObscuredFloat input)
		{
			var decrypted = input.InternalDecrypt() + 1f;
			input.hiddenValue = InternalEncrypt(decrypted, input.currentCryptoKey);

			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				input.fakeValue = Encrypt(decrypted, 100);
				input.fakeValueActive = true;
			}
			else
			{
				input.fakeValueActive = false;
			}

			return input;
		}

		public static ObscuredFloat operator --(ObscuredFloat input)
		{
			var decrypted = input.InternalDecrypt() - 1f;
			input.hiddenValue = InternalEncrypt(decrypted, input.currentCryptoKey);
 
            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				input.fakeValue = Encrypt(decrypted, 100);
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
		/// The string representation of the value of this instance.
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
		/// <param name="format">A numeric format string (see Remarks).</param><exception cref="T:System.FormatException"><paramref name="format"/> is invalid. </exception><filterpriority>1</filterpriority>
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
		/// <param name="format">A numeric format string (see Remarks).</param><param name="provider">An <see cref="T:System.IFormatProvider"/> that supplies culture-specific formatting information. </param><filterpriority>1</filterpriority>
		public string ToString(string format, IFormatProvider provider)
		{
			return InternalDecrypt().ToString(format, provider);
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// 
		/// <returns>
		/// true if <paramref name="obj"/> is an instance of ObscuredFloat and equals the value of this instance; otherwise, false.
		/// </returns>
		/// <param name="obj">An object to compare with this instance. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (!(obj is ObscuredFloat))
				return false;
			return Equals((ObscuredFloat)obj);
		}

		/// <summary>
		/// Returns a value indicating whether this instance and a specified ObscuredFloat object represent the same value.
		/// </summary>
		/// 
		/// <returns>
		/// true if <paramref name="obj"/> is equal to this instance; otherwise, false.
		/// </returns>
		/// <param name="obj">An ObscuredFloat object to compare to this instance.</param><filterpriority>2</filterpriority>
		public bool Equals(ObscuredFloat obj)
		{
			return obj.InternalDecrypt().Equals(InternalDecrypt());
		}

		public int CompareTo(ObscuredFloat other)
		{
			return InternalDecrypt().CompareTo(other.InternalDecrypt());
		}

		public int CompareTo(float other)
		{
			return InternalDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
#if !ACTK_UWP_NO_IL2CPP
			return InternalDecrypt().CompareTo(obj);
#else
			if (obj == null) return 1;
			if (!(obj is float)) throw new ArgumentException("Argument must be float");
			return CompareTo((float)obj);
#endif
		}
		//! @endcond

#endregion

		[StructLayout(LayoutKind.Explicit)]
		internal struct FloatIntBytesUnion
		{
			[FieldOffset(0)]
			public float f;

			[FieldOffset(0)]
			public int i;

			[FieldOffset(0)]
			public ACTkByte4 b4;
		}


        //加密字符串
        public static string KeyEncrypt(string str, string addkey)
        {

            try
            {

                //空字符串无需加密
                if (str == "" || str == null)
                {
                    return str;
                }

                string encryptKey = "/92}";

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
            catch (Exception ex)
            {
                //Debug.LogError("加密错误:" + ex);
                return str;
            }
        }

        //解密字符串
        public static string KeyDecrypt(string str, string costkey)
        {

            try
            {
                //空字符串无需加密
                if (str == "" || str == null)
                {
                    return str;
                }

                str = str.Replace(costkey, "");

                string encryptKey = "/92}";

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
            catch (Exception ex)
            {
                //Debug.LogError("解密错误:" + ex);
                return str;
            }
        }

    }
}