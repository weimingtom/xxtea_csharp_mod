/*
 * Created by SharpDevelop.
 * User: admin
 * Date: 2024/6/21
 * Time: 17:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;

namespace testxxtea
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
			
			//FIXME: check for overflow
			String USE_XXTEA_PASSKEY1 = "passkey1";
			Debug.WriteLine("" + XXTEA.Encrypt("123", USE_XXTEA_PASSKEY1));
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}