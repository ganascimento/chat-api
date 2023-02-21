namespace GADev.Chat.Application.Util
{
    public interface IImageStorage
    {
         void RemoveImage(string nameImage);
         string GetImage(string nameImage);
         void SaveImage(string nameImagem, string avatar);
    }
}