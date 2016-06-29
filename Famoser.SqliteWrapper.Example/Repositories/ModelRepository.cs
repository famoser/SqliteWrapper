﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.SqliteWrapper.Attributes;
using Famoser.SqliteWrapper.Converters;
using Famoser.SqliteWrapper.Example.Converters;
using Famoser.SqliteWrapper.Example.Entities;
using Famoser.SqliteWrapper.Example.Implementations;
using Famoser.SqliteWrapper.Example.Models;
using Famoser.SqliteWrapper.Repositories;
using Famoser.SqliteWrapper.Services;
using SQLite.Net.Platform.WinRT;

namespace Famoser.SqliteWrapper.Example.Repositories
{
    public class ModelRepository
    {
        private readonly GenericRepository<MyModel, MyBaseEntity> _genericRepo;

        public ModelRepository()
        {
            //register additional converters
            EntityConversionAttribute.RegisterConverter(new ListStringToStringConverter(), typeof(string), typeof(List<string>));
            EntityConversionAttribute.RegisterConverter(new GuidStringConverter(), typeof(string), typeof(Guid));

            //construct the sqlite service, it is recommended to do this with an IoC approach
            var service = new SqliteService(new SQLitePlatformWinRT(), new SqliteServiceSettingsProvider());
            //construct generic repository
            _genericRepo = new GenericRepository<MyModel, MyBaseEntity>(service);
        }

        /// <summary>
        /// retrieves model this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MyModel> GetById(int id)
        {
            return await _genericRepo.GetById(id);
        }

        /// <summary>
        /// Adds or Updates this model in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Save(MyModel model)
        {
            return await _genericRepo.Save(model);
        }

        /// <summary>
        /// Update the model in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Update(MyModel model)
        {
            return await _genericRepo.Update(model);
        }

        /// <summary>
        /// Removes the model from the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Delete(MyModel model)
        {
            return await _genericRepo.Delete(model);
        }
    }
}
