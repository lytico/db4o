namespace Sharpen.IO
{
	public interface IInputStream
	{
		int Read();

		int Read(byte[] bytes);

		int Read(byte[] bytes, int offset, int length);

		void Close();
	}
}
