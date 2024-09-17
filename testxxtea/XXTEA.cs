/*
 * Created by SharpDevelop.
 * User: admin
 * Date: 2024/6/21
 * Time: 14:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

//https://blog.csdn.net/jdsjlzx/article/details/51869353

namespace testxxtea
{
	/*
	* XXTEA 加密算法
	*/
	//https://blog.csdn.net/jdsjlzx/article/details/51869353
	public class XXTEA
	{
		private static readonly UTF8Encoding utf8 = new UTF8Encoding();
		
		public static String Encrypt(String data, String key) {
		      return ToHexString(TEAEncrypt(
		              ToLongArray(utf8.GetBytes(PadRight(data, MIN_LENGTH)
				                          )),
		                      ToLongArray(utf8.GetBytes(PadRight(key, MIN_LENGTH)
				                         ))));
		  }
		
		  public static String Decrypt(String data, String key) {
		      if (data == null || data.Length < MIN_LENGTH) {
		          return data;
		      }
		      byte[] code = ToByteArray(TEADecrypt(
		              ToLongArray(data),
		              ToLongArray(utf8.GetBytes(PadRight(key, MIN_LENGTH)
		                                       ))));
		      return utf8.GetString(code);
		  }
		
		  private static long[] TEAEncrypt(long[] data, long[] key) {
			for (int kk = 0; kk < data.Length; ++kk)
			{
				Debug.WriteLine("TEAEncrypt data:" + kk + "=>" + data[kk]);
			}
			for (int kk = 0; kk < key.Length; ++kk)
			{
				Debug.WriteLine("TEAEncrypt key:" + kk + "=>" + key[kk]);
			}
			
		      int n = data.Length;
		      if (n < 1) {
		          return data;
		      }
		
		      long z = data[data.Length - 1], y = data[0], sum = 0, e, p, q;
		      q = (6 + 52 / n);
		      while (q-- > 0) {
		      	sum = (sum + (long)(Int32)DELTA);
		          e = (sum >> 2) & 3;
		          //Debug.WriteLine("e:" + e);
		          for (p = 0; p < (n - 1); p++) {
		          	  y = data[(int) (p + 1)];
//		              Debug.WriteLine("pre.z:" + z + ", data[(int) p], " + data[(int) p]);
//		              Debug.WriteLine("kkkk.pre1:" + (long)((z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)));
//		              Debug.WriteLine("kkkk.pre2:" + (long)((((((long)((sum ^ y))))))) + ", sum == " + sum + ", y == " + y);
//		              Debug.WriteLine("kkkk.pre2:" + (long)((((((long)(((long)sum ^ (long)y) + (long)key[(int) (p & 3 ^ e)]) & 0xffffffff)))) & 0xffffffff));
//		              Debug.WriteLine("kkkk.pre2:" + (long)((((((long)(((long)sum ^ (long)y) + (long)key[(int) (p & 3 ^ e)]) & 0xffffffff) ^ (long)(z & 0xffffffff)))) & 0xffffffff));

		              long kkkk = (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)
                      	^ (sum ^ y) + (key[(int) (p & 3 ^ e)] ^ z);
//		              Debug.WriteLine("kkkk:" + kkkk);
		          	  z = (data[(int) p] = (data[(int) p] + kkkk));
		              Debug.WriteLine("z:" + z + ", data[(int) p], " + data[(int) p]);
		          }
		          y = data[0];
		          z = data[(int) p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)
                      ^ (sum ^ y) + (key[(int) (p & 3 ^ e)] ^ z);
		          Debug.WriteLine("z:" + z);
		      }
/*
z:-1637176342, data[(int) p], -1637176342
z:29515069020, data[(int) p], 29515069020
z:8319099694440551066, data[(int) p], 8319099694440551066
z:-5306029298206350755
z:-6437820955724342261, data[(int) p], -6437820955724342261 <-到这里开始不对
z:-8072495025158169473, data[(int) p], -8072495025158169473
z:-2625047403164599914, data[(int) p], -2625047403164599914
*/ 		
		      
			for (int kk = 0; kk < data.Length; ++kk)
			{
				Debug.WriteLine("TEAEncrypt return:" + kk + "=>" + data[kk]);
			}
			
		      return data;
		  }
		
		  private static long[] TEADecrypt(long[] data, long[] key) {
		      int n = data.Length;
		      if (n < 1) {
		          return data;
		      }
		
		      long z = data[data.Length - 1], y = data[0], sum = 0, e, p, q;
		      q = 6 + 52 / n;
		      sum = q * DELTA;
		      while (sum != 0) {
		          e = (sum >> 2) & 3;
		          for (p = n - 1; p > 0; p--) {
		              z = data[(int) (p - 1)];
		              y = data[(int) p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4)
		                      ^ (sum ^ y) + (key[(int) (p & 3 ^ e)] ^ z);
		          }
		          z = data[n - 1];
		          y = data[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y)
		                  + (key[(int) (p & 3 ^ e)] ^ z);
		          sum -= DELTA;
		      }
		
		      return data;
		  }
		
		  private static long[] ToLongArray(byte[] data) {
		      int n = (data.Length % 8 == 0 ? 0 : 1) + data.Length / 8;
		      long[] result = new long[n];
		
		      for (int i = 0; i < n - 1; i++) {
		          result[i] = bytes2long(data, i * 8);
		      }
		
		      byte[] buffer = new byte[8];
		      for (int i = 0, j = (n - 1) * 8; j < data.Length; i++, j++) {
		          buffer[i] = data[j];
		      }
		      result[n - 1] = bytes2long(buffer, 0);
		
		      return result;
		  }
		
		  private static byte[] ToByteArray(long[] data) {
		      List<Byte> result = new List<Byte>();
		
		      for (int i = 0; i < data.Length; i++) {
		          byte[] bs = long2bytes(data[i]);
		          for (int j = 0; j < 8; j++) {
		              result.Add(bs[j]);
		          }
		      }
		
		      while (result[result.Count - 1] == SPECIAL_CHAR) {
		          result.RemoveAt(result.Count - 1);
		      }
		
		      byte[] ret = new byte[result.Count];
		      for (int i = 0; i < ret.Length; i++) {
		      		ret[i] = result[i];
		      }
		      return ret;
		  }
		
		  public static byte[] long2bytes(long num) {
				return BitConverter.GetBytes(num);
		  }
		
		  public static long bytes2long(byte[] b, int index) {
				return BitConverter.ToInt64(b, index);
		  }
		
		  private static String ToHexString(long[] data) {
		      StringBuilder sb = new StringBuilder();
		      for (int i = 0; i < data.Length; i++) {
		      	byte[] bytes = BitConverter.GetBytes(data[i]);
		      	byte[] bytes2 = new byte[bytes.Length];
		      	for (int k = 0; k < bytes.Length; ++k)
		      	{
		      		bytes2[bytes2.Length - k - 1] = bytes[k];
		      	}
		      	sb.Append(PadLeft(BitConverter.ToString(bytes2).Replace("-", ""), 16));
		      }
		      return sb.ToString();
		  }
		
		  private static long[] ToLongArray(String data) {
		      int len = data.Length / 16;
		      long[] result = new long[len];
		      for (int i = 0; i < len; i++) {
		          result[i] = long.Parse(data.Substring(i * 16, i * 16 + 16), System.Globalization.NumberStyles.AllowHexSpecifier);
		      }
		      return result;
		  }
		
		  private static String PadRight(String source, int length) {
		      while (source.Length < length) {
		          source += SPECIAL_CHAR;
		      }
		      return source;
		  }
		
		  private static String PadLeft(String source, int length) {
		      while (source.Length < length) {
		          source = '0' + source;
		      }
		      return source;
		  }
		
		  private static long DELTA = 0x9E3779B9;
		  private static int MIN_LENGTH = 32;
		  private static char SPECIAL_CHAR = '\0';
	}
}
