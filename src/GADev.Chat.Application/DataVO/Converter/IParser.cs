using System.Collections.Generic;

namespace GADev.Chat.Application.DataVO.Converter
{
    public interface IParser<D, O>
    {
         D Parse (O origin);
         List<D> ParseList(List<O> origin);
    }
}