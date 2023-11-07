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
	using UnityEngine;
	using Utils;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Use it instead of regular <c>long</c> for any cheating-sensitive variables.
    /// </summary>
    /// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong>
    [Serializable]
	public struct ObscuredLong : IFormattable, IEquatable<ObscuredLong>, IComparable<ObscuredLong>, IComparable<long>, IComparable
	{

		class CheckObLong
		{
			private string _checkint;

			public string CheckInt
			{

				get { return _checkint; }
				set { _checkint = value; }

			}

			public CheckObLong()
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

        private static long cryptoKey;

#if UNITY_EDITOR
		// For internal Editor usage only (may be useful for drawers).
		public static long cryptoKeyEditor = cryptoKey;
#endif
		[SerializeField]
		private long currentCryptoKey;

		[SerializeField]
		private bool fakeValueActive;

        [SerializeField]
        private string hiddenValueStr;      //加密字符串

        [SerializeField]
        private string fakeValueStr;        //解密字符串

		[SerializeField]
		private long fakeValue;

		[SerializeField]
        private string hiddenValueAddStr;      //加密字符串 再加密

		[SerializeField]
		private bool fakeValueActiveEx;     //自定义效验参数

		private string checkInit;

		[SerializeField]
		private long hiddenValue;

		[SerializeField]
		private bool inited;


		private CheckObLong checkObLong;

		private ObscuredLong(long value)
		{

            //初始化
            cryptoKey = cryptoKey111.KeyValue;
            //cryptoKeyEditor = cryptoKey;
            currentCryptoKey = cryptoKey;
            hiddenValueStr = "";
            fakeValueStr = "";

            //currentCryptoKey = cryptoKey;
			hiddenValue = Encrypt(value);
            hiddenValueStr = hiddenValue.ToString();

			checkInit = currentCryptoKey.ToString();

			hiddenValueAddStr = "";
            //hiddenValueAddStr = KeyEncrypt(hiddenValueStr, currentCryptoKey.ToString());

			checkObLong = new CheckObLong();
			//hiddenValueAddStr = KeyEncrypt(hiddenValueStr, currentCryptoKey.ToString());

#if UNITY_EDITOR
			fakeValue = value + currentCryptoKey;
			fakeValueActive = true;
			fakeValueActiveEx = true;
#else
            /*
			var detectorRunning = Detectors.ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = (detectorRunning ? value : 0 );
			fakeValue = fakeValue + currentCryptoKey;
			fakeValueActive = detectorRunning;
            */

            var detectorRunning = Detectors.ObscuredCheatingDetector.ExistsAndIsRunning;
            fakeValue = (detectorRunning ? value : value) ;
			fakeValue = fakeValue + currentCryptoKey;
            fakeValueActive = detectorRunning;
			fakeValueActiveEx = detectorRunning;

#endif

			inited = true;
		}

		/// <summary>
		/// Allows to change default crypto key of this type instances. All new instances will use specified key.<br/>
		/// All current instances will use previous key unless you call ApplyNewCryptoKey() on them explicitly.
		/// </summary>
		public static void SetNewCryptoKey(long newKey)
		{
			cryptoKey = newKey;
		}

		/// <summary>
		/// Simple symmetric encryption, uses default crypto key.
		/// </summary>
		/// <returns>Encrypted <c>long</c>.</returns>
		public static long Encrypt(long value)
		{
			return Encrypt(value, 0);
		}

		/// <summary>
		/// Simple symmetric encryption, uses default crypto key.
		/// </summary>
		/// <returns>Decrypted <c>long</c>.</returns>
		public static long Decrypt(long value)
		{
			return Decrypt(value, 0);
		}

		/// <summary>
		/// Simple symmetric encryption, uses passed crypto key.
		/// </summary>
		/// <returns>Encrypted <c>long</c>.</returns>
		public static long Encrypt(long value, long key)
		{
			if (key == 0)
			{
				return value ^ cryptoKey;
			}
			return value ^ key;
		}

		/// <summary>
		/// Simple symmetric encryption, uses passed crypto key.
		/// </summary>
		/// <returns>Decrypted <c>long</c>.</returns>
		public static long Decrypt(long value, long key)
		{
			if (key == 0)
			{
				return value ^ cryptoKey;
			}
			return value ^ key;
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
		public static ObscuredLong FromEncrypted(long encrypted)
		{
			var instance = new ObscuredLong();
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
			var decrypted = InternalDecrypt();
			currentCryptoKey = ThreadSafeRandom.Next();
			hiddenValue = Encrypt(decrypted, currentCryptoKey);
		}

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		public long GetEncrypted()
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
		public void SetEncrypted(long encrypted)
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

		/// <summary>
		/// Alternative to the type cast, use if you wish to get decrypted value 
		/// but can't or don't want to use cast to the regular type.
		/// </summary>
		/// <returns>Decrypted value.</returns>
		public long GetDecrypted()
		{
			return InternalDecrypt();
		}

		private long InternalDecrypt()
		{

			if (currentCryptoKey != 0 && hiddenValueStr != null && hiddenValueStr != "")
			{
				fakeValueActive = true;
				fakeValueActiveEx = true;
			}

			if (inited == false && checkInit == null || checkInit.Length == 0)
			{
				currentCryptoKey = cryptoKey;
				hiddenValue = Encrypt(0);
				hiddenValueStr = hiddenValue.ToString();
				fakeValue = 0 + currentCryptoKey;
				fakeValueActive = false;
				fakeValueActiveEx = false;
				checkInit = currentCryptoKey.ToString();
				inited = true;

				return 0;
			}

			var decrypted = Decrypt(hiddenValue, currentCryptoKey);
            fakeValueStr = decrypted.ToString();

            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != (fakeValue - currentCryptoKey))
			{
				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}

            bool ifSendStatus = true;

			if (currentCryptoKey < 10000 && checkInit != null )
			{
				//Debug.LogError("currentCryptoKey < 10000");
				//Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
				ifSendStatus = false;
			}

			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActiveEx && hiddenValueStr != hiddenValue.ToString() && hiddenValueStr != "" && hiddenValueStr != null)
            {
                ifSendStatus = false;
                //Debug.LogError("数据验证失败" + "hiddenValueStr = " + hiddenValueStr + "hiddenValue = " + hiddenValue);
            }


            if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActiveEx && fakeValueStr != (fakeValue - currentCryptoKey).ToString() && fakeValueStr != "" && fakeValueStr != null)
            {
                ifSendStatus = false;
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
                    long costValue = Decrypt(long.Parse(coststr), currentCryptoKey);
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

            if (ifSendStatus)
            {
                return decrypted;
            }
            else
            {
				//Debug.LogError("long数据验证失败,存在修改内容" + "读取值 hiddenValue = " + hiddenValue + "currentCryptoKey = " + currentCryptoKey + "decrypted = " + decrypted + "fakeValue = " + fakeValue);
				Debug.LogError("数据验证失败,存在修改内容..");
				//Application.Quit();     //强制退出
				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
                return 0;
            }

            return 1;


            //return decrypted;
        }

#region operators, overrides, interface implementations
		//! @cond
		public static implicit operator ObscuredLong(long value)
		{
			return new ObscuredLong(value);
		}

		public static implicit operator long(ObscuredLong value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredLong operator ++(ObscuredLong input)
		{
			var decrypted = input.InternalDecrypt() + 1L;
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

		public static ObscuredLong operator --(ObscuredLong input)
		{
			var decrypted = input.InternalDecrypt() - 1L;
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
		/// true if <paramref name="obj"/> is an instance of ObscuredLong and equals the value of this instance; otherwise, false.
		/// </returns>
		/// <param name="obj">An object to compare with this instance. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (!(obj is ObscuredLong))
				return false;
			return Equals((ObscuredLong)obj);
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified ObscuredLong value.
		/// </summary>
		/// 
		/// <returns>
		/// true if <paramref name="obj"/> has the same value as this instance; otherwise, false.
		/// </returns>
		/// <param name="obj">An ObscuredLong value to compare to this instance.</param><filterpriority>2</filterpriority>
		public bool Equals(ObscuredLong obj)
		{
			if (currentCryptoKey == obj.currentCryptoKey)
			{
				return hiddenValue == obj.hiddenValue;
			}

			return Decrypt(hiddenValue, currentCryptoKey) == Decrypt(obj.hiddenValue, obj.currentCryptoKey);
		}

		public int CompareTo(ObscuredLong other)
		{
			return InternalDecrypt().CompareTo(other.InternalDecrypt());
		}

		public int CompareTo(long other)
		{
			return InternalDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
#if !ACTK_UWP_NO_IL2CPP
			return InternalDecrypt().CompareTo(obj);
#else
			if (obj == null) return 1;
			if (!(obj is long)) throw new ArgumentException("Argument must be long");
			return CompareTo((long)obj);
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
                Debug.LogError("加密错误:" + ex);
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
                Debug.LogError("解密错误:" + ex);
                return str;
            }
        }

    }
}
