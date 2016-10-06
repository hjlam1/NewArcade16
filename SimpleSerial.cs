using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SimpleSerial : MonoBehaviour {

	private SerialPort serialPort = null;
	private String portName = "/dev/tty.usbmodem1411";
	private int baudRate =  115200;             
	private int readTimeOut = 100;                  

	private string serialInput;

	bool programActive = true;
	Thread thread;

	void Start () {
		try
		{
			serialPort = new SerialPort();
			serialPort.PortName = portName;
			serialPort.BaudRate = baudRate;
			serialPort.ReadTimeout = readTimeOut;   
			serialPort.Open();
		}
		catch (Exception e) {
			Debug.Log (e.Message);
		}
		thread = new Thread(new ThreadStart(ProcessData));
		thread.Start();
	}

	void ProcessData(){
		Debug.Log ("Thread: Start");
		while (programActive) {
			try {
				serialInput = serialPort.ReadLine();
			} catch (TimeoutException) {

			}
		}
		Debug.Log ("Thread: Stop");
	}

	void Update () {
		if (serialInput != null) {
			string[] strEul= serialInput.Split (',');
			if (strEul.Length > 0) {
				if (int.Parse (strEul [0]) != 0) {
					this.GetComponent<MeshRenderer> ().enabled = true;	

				} else {
					this.GetComponent<MeshRenderer> ().enabled = false;

				}
			}
		}
	}

	public void OnDisable(){
		programActive = false;   
		if (serialPort != null && serialPort.IsOpen) 
			serialPort.Close ();
	}
}
