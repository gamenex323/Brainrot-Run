using System.Collections;
using UnityEngine;

public class DelayedCamera : MonoBehaviour
{
	public struct Frame
	{
		private Texture2D texture;

		private float recordedTime;

		public void CaptureFrom(RenderTexture renderTexture)
		{
			RenderTexture.active = renderTexture;
			if (texture == null)
			{
				texture = new Texture2D(renderTexture.width, renderTexture.height);
			}
			texture.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
			texture.Apply();
			recordedTime = Time.time;
			RenderTexture.active = null;
		}

		public bool CapturedBefore(float time)
		{
			return recordedTime < time;
		}

		public static implicit operator Texture2D(Frame frame)
		{
			return frame.texture;
		}
	}

	[SerializeField]
	private Camera renderCamera;

	[SerializeField]
	private float delay = 0.5f;

	private int bufferSize = 256;

	private RenderTexture renderTexture;

	private Frame[] frames;

	private int capturedFrameIndex;

	private int renderedFrameIndex;

	private int frameIndex;

	private void Awake()
	{
		frames = new Frame[bufferSize];
		renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
		renderCamera.targetTexture = renderTexture;
		StartCoroutine(Render());
	}

	private IEnumerator Render()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		while (true)
		{
			yield return wait;
			capturedFrameIndex = frameIndex % bufferSize;
			frames[capturedFrameIndex].CaptureFrom(renderTexture);
			while (frames[renderedFrameIndex].CapturedBefore(Time.time - delay))
			{
				renderedFrameIndex = (renderedFrameIndex + 1) % bufferSize;
			}
			Graphics.Blit((Texture)(Texture2D)frames[renderedFrameIndex], (RenderTexture)null);
			frameIndex++;
		}
	}
}
