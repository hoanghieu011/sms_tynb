﻿using SMS_TYNB.Models.Master;

namespace SMS_TYNB.Service
{
	public interface IWpFileService
	{
		Task<WpFile?> GetById(int id);
		Task<WpFile> Create(WpFile model);
		Task<WpFile?> Update(WpFile model);
		Task Delete(WpFile model);
		Task<IEnumerable<WpFile>> GetAll();
		Task<IEnumerable<WpFile>> GetByBangLuuFile(string tableName);
	}
}
