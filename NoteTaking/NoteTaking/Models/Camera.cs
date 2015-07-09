using System;
using Xamarin.Forms;
using NoteTaking;
using System.Threading.Tasks;

namespace NoteTaking
{
	public class Camera
	{
		public Camera ()
		{
			Setup ();
		}

		private IMediaPicker _mediaPicker;

		private ImageSource _imageSource;

		private Command _takePictureCommand;
		private Command _selectPictureCommand;

		public string FileUri { set; get; }

		private async Task TakePicture ()
		{
			Setup();

			ImageSource = null;

			await this._mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400}).ContinueWith(t =>
				{
					if (t.IsFaulted)
					{
						var s = t.Exception.InnerException.ToString();
					}
					else if (t.IsCanceled)
					{
						var canceled = true;
					}
					else
					{
						var mediaFile = t.Result;

						ImageSource = ImageSource.FromStream(() => mediaFile.Source);

						return mediaFile;
					}

					return null;
				}, _scheduler);
		}
	}
}

