using System;
using System.Threading.Tasks;
using GADev.Chat.Application.DataVO.Convertes;
using GADev.Chat.Application.DataVO.VO;
using GADev.Chat.Application.Repositories;
using GADev.Chat.Application.Util;

namespace GADev.Chat.Application.Business.Implementations
{
    public class TalkBusiness : ITalkBusiness
    {
        private ITalkRepository _talkRepository;
        private TalkConverter _talkConverter;

        public TalkBusiness(ITalkRepository talkRepository, IImageStorage imageStorage)
        {
            _talkRepository = talkRepository;
            _talkConverter = new TalkConverter(imageStorage);
        }

        public async Task<TalkVO> GetInitialData(int userId)
        {
            try {
                return _talkConverter.Parse(await _talkRepository.GetInitialData(userId));
            } catch {
                return null;
            }
        }
    }
}