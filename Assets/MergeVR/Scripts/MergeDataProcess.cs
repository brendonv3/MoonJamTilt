using System;
using UnityEngine;

namespace WinDataProcess
{
	public class MergeDataProcess {

		float qua_Conversion = 16384.0f;
		float lia_Conversion = 100.0f;
		float joy_Conversion = 1000.0f;
		float y_Intercept = -3.396f;
		float slope = 1.418f;
		float joyLowPassFilter = 0.19f;

		public enum DataTypeEnum{
			qua,lia,joy
		}
		public float ProcessData(byte[] dataIn,DataTypeEnum dataType){
			switch (dataType) {
			case DataTypeEnum.qua:
				return ProcessQua (dataIn);
			case DataTypeEnum.lia:
				return ProcessLia (dataIn);
			case DataTypeEnum.joy:
				return ProcessJoy (dataIn);
			}
			return 0;
		}
		float ProcessQua(byte[] dataIn){
			return ByteToFlow (dataIn)/qua_Conversion;
		}
		float ProcessLia(byte[] dataIn){
			return ByteToFlow (dataIn)/lia_Conversion;
		}
		float ProcessJoy(byte[] dataIn){
			float returnData = (ByteToFlow (dataIn)/joy_Conversion)*slope+y_Intercept;
			if (Mathf.Abs (returnData) < joyLowPassFilter) {
				returnData = 0;
			}

			if (returnData < -1f) {
				returnData = -1f;
			}
			else if (returnData > .94f) {
				returnData = 1f;
			}
			return returnData;
		}
		public UInt16 ProcessBtn(byte[] dataIn){
			return ByteToUInd (dataIn);
		}
		float ByteToFlow(byte [] toConvert){
			float returnValue = (float)(BitConverter.ToInt16 (toConvert,0));
			return returnValue;
		}
		UInt16 ByteToUInd(byte [] toConvert){
			UInt16 returnValue = (UInt16)(BitConverter.ToInt16 (toConvert,0));
			return returnValue;
		}
	}
}

