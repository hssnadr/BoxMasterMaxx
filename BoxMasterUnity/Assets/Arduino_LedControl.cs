using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Arduino_LedControl : MonoBehaviour
{
	public SerialPort serial;
	// arduino serial port

	// Led pannel
	int ROWS = 7;
	int COLS = 20;
	Color[] leds; // store leds data (Red, Green, Blue)
	Texture2D screenTexture;

	// Led variables
	public int iPix = 0;
	public int red = 0;
	public int green = 0;
	public int blue = 0;

	void Start ()
	{
		// Initialize serial connection to leds pannel
		serial = new SerialPort ("COM5", 38400);
		Debug.Log ("Connection started");
		try {
			serial.Open ();
			serial.ReadTimeout = 400;
			serial.Handshake = Handshake.None;
			Debug.Log ("Port Opened!");
		} catch {
			Debug.Log ("Could not open serial port");
		}

		// Initialize leds array to store color values
		leds = new Color[ROWS * COLS];
		for (int i = 0; i < COLS; i++) {
			for (int j = 0; j < ROWS; j++) {
				setLedColor (i,j,Color.black);
			}
		}
	}

	public void OnApplicationQuit ()
	{
		if (serial != null) {
			if (serial.IsOpen) {
				print ("closing serial port");
				serial.Close ();
			}
			serial = null;
		}
	}

	void Update ()
	{
		// Get pixel color from camera and send it to the corresponding led on the leds strip
		screenTexture = ScreenCapture.CaptureScreenshotAsTexture (); 
		for (int i = 0; i < COLS; i ++) {
			for (int j = 0; j < ROWS; j ++) {
				int gx_ = (int)(screenTexture.width * (0.8f * i / ((float)COLS) + 0.1f));
				int gy_ = (int)(screenTexture.height * (0.8f * j / ((float)ROWS) + 0.1f));
				setLedColor (i,j,screenTexture.GetPixel (gx_ ,gy_));
			}
		}
	}

	private int getLedIndex (int x_, int y_)
	{
		// Convert X and Y coordinate into the led index
		x_ = Mathf.Clamp (x_, 0, COLS - 1);
		y_ = Mathf.Clamp (y_, 0, ROWS - 1);
		if (y_ % 2 != 0) {
			x_ = COLS-1 - x_;
		}
		y_ = ROWS-1 - y_;
		int ipix_ = y_ * COLS + x_;
		return ipix_;
	}

	public void setLedColor (int x_, int y_, Color col_)
	{
		int ipix_ = getLedIndex (x_, y_); // get led index on the strip

		// Do not send color value to the led if it's already the same
		if (leds [ipix_] != col_) {
			int r_ = (int)(255 * col_.r);
			int g_ = (int)(255 * col_.g);
			int b_ = (int)(255 * col_.b);

			// Create data string to send and send it to serial port
			//string data = "" + System.Convert.ToChar (ipix_) + System.Convert.ToChar (r_) + System.Convert.ToChar (g_) + System.Convert.ToChar (b_) + '_';
			string data = "" + ipix_.ToString("000") + r_.ToString("000") + g_.ToString("000") + b_.ToString("000") + '_';
			//print (data);

			if (serial.IsOpen) {
				serial.Write (data);
				leds [ipix_] = col_; // update led color values
			}
		}
	}
}