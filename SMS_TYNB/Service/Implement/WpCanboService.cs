﻿using Microsoft.EntityFrameworkCore;
using SMS_TYNB.Common;
using SMS_TYNB.Models.Master;
using SMS_TYNB.Repository;
using SMS_TYNB.ViewModel;

namespace SMS_TYNB.Service.Implement
{
	public class WpCanboService : IWpCanboService
	{
		private readonly WpCanboRepository _wpCanboRepository;
		private readonly WpDanhmucRepository _wpDanhmucRepository;
		public WpCanboService(WpCanboRepository wpCanboRepository, WpDanhmucRepository wpDanhmucRepository) {
			_wpCanboRepository = wpCanboRepository;
			_wpDanhmucRepository = wpDanhmucRepository;
		}
		public async Task<IEnumerable<WpCanbo>> GetAllWpCanbo()
		{
			IEnumerable<WpCanbo> wpCanbos = await _wpCanboRepository.GetAll();
			return wpCanbos;
		}
		public async Task<PageResult<WpCanboViewModel>> SearchWpCanbo(WpCanboSearchViewModel model, Pageable pageable)
		{
			IQueryable<WpCanbo> wpCanbos = await _wpCanboRepository.Search(model.searchInput);
			var wpCanbosPage = await _wpCanboRepository.GetPagination(wpCanbos, pageable);

			var wpNhomsViewModel = from wpcb in wpCanbosPage
								   join wpdmtt in await _wpDanhmucRepository.GetByType("TRANGTHAI") on wpcb.Trangthai equals wpdmtt.MaDanhmuc
								   join wpdmgt in await _wpDanhmucRepository.GetByType("GIOITINH") on wpcb.Gioitinh equals wpdmgt.MaDanhmuc
								   where (wpcb.Trangthai == model.Trangthai || model.Trangthai == null)
								   select new WpCanboViewModel
								   {
									   IdCanbo = wpcb.IdCanbo,
									   MaCanbo = wpcb.MaCanbo,
									   TenCanbo = wpcb.TenCanbo,
									   Gioitinh = wpdmgt.TenDanhmuc,
									   SoDt = wpcb.SoDt,
									   Mota = wpcb.Mota,
									   Trangthai = wpdmtt.TenDanhmuc
								   };

			int total = await wpCanbos.CountAsync();

			return new PageResult<WpCanboViewModel> { 
				Data = wpNhomsViewModel,
				Total = total,
			};
		}
		public async Task<WpCanbo?> GetById(int wpCanboId)
		{
			WpCanbo? wpCanbo = await _wpCanboRepository.FindById(wpCanboId);
			return wpCanbo;
		}
		public async Task<WpCanbo> Create(WpCanbo wpCanbo)
		{
			WpCanbo wpCanboNew = await _wpCanboRepository.Create(wpCanbo);
			return wpCanboNew;
		}
		public async Task<WpCanbo?> Update(WpCanbo wpCanbo)
		{
			WpCanbo? wpCanboUpdated = await _wpCanboRepository.Update(wpCanbo.IdCanbo, wpCanbo);
			return wpCanboUpdated;
		}
		public async Task Delete(WpCanbo wpCanbo)
		{
			await _wpCanboRepository.Delete(wpCanbo.IdCanbo);
		}
	}
}
