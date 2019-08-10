﻿using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class KPILevelDAO
    {
        KPIDbContext _dbContext = null;
        public KPILevelDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Update các cột WeeklyChecked, MonthlyChecked, QuaterlyChecked, YearlyChecked</returns>
        public bool Update(EF.KPILevel entity)
        {
            var comparedt = new DateTime(2001, 1, 1);
            var kpiLevel = _dbContext.KPILevels.FirstOrDefault(x => x.KPIID == entity.KPIID && x.LevelID == entity.LevelID);
            if (entity.Weekly != null)
            {
                kpiLevel.Weekly = entity.Weekly;
            }
            if (DateTime.Compare(entity.Monthly.Value, comparedt) != 0)
            {
                kpiLevel.Monthly = entity.Monthly;
            }
            if (DateTime.Compare(entity.Quaterly.Value, comparedt) != 0)
            {
                kpiLevel.Quaterly = entity.Quaterly;
            }
            if (DateTime.Compare(entity.Yearly.Value, comparedt) != 0)
            {
                kpiLevel.Yearly = entity.Yearly;
            }
            if (entity.WeeklyChecked != null)
            {
                kpiLevel.WeeklyChecked = entity.WeeklyChecked;
            }
            if (entity.MonthlyChecked != null)
            {
                kpiLevel.MonthlyChecked = entity.MonthlyChecked;
            }
            if (entity.QuaterlyChecked != null)
            {
                kpiLevel.QuaterlyChecked = entity.QuaterlyChecked;
            }
            if (entity.MonthlyChecked != null)
            {
                kpiLevel.MonthlyChecked = entity.MonthlyChecked;
            }
            if (entity.YearlyChecked != null)
            {
                kpiLevel.YearlyChecked = entity.YearlyChecked;
            }
            if (entity.WeeklyPublic != null)
            {
                kpiLevel.WeeklyPublic = entity.WeeklyPublic;
            }
            if (entity.MonthlyPublic != null)
            {
                kpiLevel.MonthlyPublic = entity.MonthlyPublic;
            }
            if (entity.QuaterlyPublic != null)
            {
                kpiLevel.QuaterlyPublic = entity.QuaterlyPublic;
            }
            if (entity.YearlyPublic != null)
            {
                kpiLevel.YearlyPublic = entity.YearlyPublic;
            }
            if (entity.Checked != null)
            {
                kpiLevel.Checked = entity.Checked;
                kpiLevel.KPILevelCode = entity.KPILevelCode;
            }

            kpiLevel.UserCheck = entity.UserCheck;
            kpiLevel.TimeCheck = entity.TimeCheck;
            //kpiLevel.Code = entity.Code;

            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //logging
                return false;
            }

        }
        public int Total()
        {
            return _dbContext.KPILevels.Where(x => x.Checked == true).ToList().Count();
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns>Lấy danh sách các KPILevel</returns>
        public IEnumerable<EF.KPILevel> GetAll()
        {
            return _dbContext.KPILevels.ToList();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns>Dnah sách các level theo điều kiện</returns>
        public IEnumerable<EF.KPILevel> GetAllByID(int levelID)
        {
            return _dbContext.KPILevels.Where(x => x.LevelID == levelID).ToList();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Tìm kiếm KPILevel theo ID</returns>
        public EF.KPILevel GetbyID(int ID)
        {
            return _dbContext.KPILevels.FirstOrDefault(x => x.ID == ID);
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns>Danh sách tất cả các record trong bảng Category</returns>
        public IEnumerable<EF.Category> GetAllCategory()
        {
            return _dbContext.Categories.ToList();
        }
        /// <summary>
        /// Lấy ra danh sách tất cả các KPILevel
        /// </summary>
        /// <param name="levelID"></param>
        /// <param name="categoryID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>Danh sách KPI theo điều kiện</returns>
        public object LoadData(int levelID, int categoryID, int page, int pageSize = 3)
        {
            var model = from kpiLevel in _dbContext.KPILevels
                        where kpiLevel.LevelID == levelID
                        join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                        join level in _dbContext.Levels on kpiLevel.LevelID equals level.ID
                        select new ViewModel.KPILevelVM
                        {
                            ID = kpiLevel.ID,
                            KPILevelCode = kpiLevel.KPILevelCode,
                            UserCheck = kpiLevel.KPILevelCode,
                            KPIID = kpiLevel.KPIID,
                            KPICode = kpi.Code,
                            LevelID = kpiLevel.LevelID,
                            LevelNumber = kpi.LevelID,
                            Period = kpiLevel.Period,

                            Weekly = kpiLevel.Weekly,
                            Monthly = kpiLevel.Monthly,
                            Quaterly = kpiLevel.Quaterly,
                            Yearly = kpiLevel.Yearly,

                            Checked = kpiLevel.Checked,
                            WeeklyChecked = kpiLevel.WeeklyChecked,
                            MonthlyChecked = kpiLevel.MonthlyChecked,
                            QuaterlyChecked = kpiLevel.QuaterlyChecked,
                            YearlyChecked = kpiLevel.YearlyChecked,
                            CheckedPeriod = kpiLevel.CheckedPeriod,

                            TimeCheck = kpiLevel.TimeCheck,

                            CreateTime = kpiLevel.CreateTime,

                            CategoryID = kpi.CategoryID,
                            KPIName = kpi.Name,
                            LevelCode = level.Code,
                        };
            if (categoryID != 0)
            {
                model = model.Where(x => x.CategoryID == categoryID);
            }

            int totalRow = model.Count();

            model = model.OrderByDescending(x => x.CreateTime)
              .Skip((page - 1) * pageSize)
              .Take(pageSize);


            return new
            {
                data = model,
                total = totalRow,
                status = true
            };
        }
        /// <summary>
        /// Lấy ra danh sách những KPI có checked bằng true.
        /// </summary>
        /// <param name="levelID"></param>
        /// <param name="categoryID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>Danh sách KPILevel có checked bằng true</returns>
        public object LoadDataForUser(int levelID, int categoryID, int page, int pageSize = 3)
        {
            //lay tuan hien tai cua nam
            var weekofyear = DateTime.Now.GetIso8601WeekOfYear();
            var monthofyear = DateTime.Now.Month;
            var quarterofyear = DateTime.Now.GetQuarterOfYear();
            var year = DateTime.Now.Year;
            var currentweekday = DateTime.Now.DayOfWeek.ToSafetyString().ToUpper().ConvertStringDayOfWeekToNumber();
            var currentdate = DateTime.Now.Date;
            var dt = new DateTime(2019, 8, 1);
            var value = DateTime.Compare(currentdate, dt);
            //bool StatusUploadDatM= _dbContext.Datas.Where(a =>
            //                    a.KPILevelCode == "1G010001PO" &&
            //                    a.KPIKind == "M")
            //                    .Max(x => x.Month) <= monthofyear ? (DateTime.Compare(currentdate, dt) < 0 ? true : false) : false;
            //bool StatusUploadDataW = _dbContext.Datas.Where(a =>
            //                  a.KPILevelCode == "1G010001PO" &&
            //                  a.KPIKind == "W")
            //                  .Max(x => x.Week) <= weekofyear ? (2 < currentweekday ? true : false) : false;
            //bool StatusUploadDataW = weekofyear - _dbContext.Datas.Where(a =>
            //                     a.KPILevelCode == "2G010002PO" &&
            //                     a.KPIKind == "W")
            //                    .Max(x => x.Week) > 1 ? false :
            //                    (weekofyear - _dbContext.Datas.Where(a =>
            //                    a.KPILevelCode == "2G010002PO" &&
            //                    a.KPIKind == "W")
            //                    .Max(x => x.Week) == 1) ? 2 < currentweekday : true;
            try
            {
                var model = from kpiLevel in _dbContext.KPILevels
                            where kpiLevel.LevelID == levelID && kpiLevel.Checked == true
                            join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                            join level in _dbContext.Levels on kpiLevel.LevelID equals level.ID
                            select new ViewModel.KPILevelVM
                            {
                                ID = kpiLevel.ID,
                                KPILevelCode = kpiLevel.KPILevelCode,
                                UserCheck = kpiLevel.KPILevelCode,
                                KPIID = kpiLevel.KPIID,
                                KPICode = kpi.Code,
                                LevelID = kpiLevel.LevelID,
                                LevelNumber = kpi.LevelID,
                                Period = kpiLevel.Period,

                                Weekly = kpiLevel.Weekly,
                                Monthly = kpiLevel.Monthly,
                                Quaterly = kpiLevel.Quaterly,
                                Yearly = kpiLevel.Yearly,

                                Checked = kpiLevel.Checked,
                                WeeklyChecked = kpiLevel.WeeklyChecked,
                                MonthlyChecked = kpiLevel.MonthlyChecked,
                                QuaterlyChecked = kpiLevel.QuaterlyChecked,
                                YearlyChecked = kpiLevel.YearlyChecked,
                                CheckedPeriod = kpiLevel.CheckedPeriod,

                                //true co du lieu false khong co du lieu
                                StatusEmptyDataW = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.KPIKind == (kpiLevel.WeeklyChecked == true ? "W" : "")) != null ? true : false,
                                StatusEmptyDataM = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.KPIKind == (kpiLevel.MonthlyChecked == true ? "M" : "")) != null ? true : false,
                                StatusEmptyDataQ = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.KPIKind == (kpiLevel.QuaterlyChecked == true ? "Q" : "")) != null ? true : false,
                                StatusEmptyDataY = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.KPIKind == (kpiLevel.YearlyChecked == true ? "Y" : "")) != null ? true : false,

                                TimeCheck = kpiLevel.TimeCheck,
                                CreateTime = kpiLevel.CreateTime,

                                CategoryID = kpi.CategoryID,
                                KPIName = kpi.Name,
                                LevelCode = level.Code,
                                //neu < 1 thi dung, 
                                StatusUploadDataW = weekofyear - _dbContext.Datas.Where(a =>
                                 a.KPILevelCode == kpiLevel.KPILevelCode &&
                                 a.KPIKind == (kpiLevel.WeeklyChecked == true ? "W" : ""))
                                .Max(x => x.Week) > 1 ? false :
                                ((weekofyear - _dbContext.Datas.Where(a =>
                                a.KPILevelCode == kpiLevel.KPILevelCode &&
                                a.KPIKind == (kpiLevel.WeeklyChecked == true ? "W" : ""))
                                .Max(x => x.Week)) == 1 ? (kpiLevel.Weekly < currentweekday ? true : false) : false),

                                StatusUploadDataM = monthofyear - _dbContext.Datas.Where(a =>
                                a.KPILevelCode == kpiLevel.KPILevelCode &&
                                a.KPIKind == (kpiLevel.MonthlyChecked == true ? "M" : ""))
                                .Max(x => x.Month) > 1 ? false : monthofyear - _dbContext.Datas.Where(a =>
                                  a.KPILevelCode == kpiLevel.KPILevelCode &&
                                  a.KPIKind == (kpiLevel.MonthlyChecked == true ? "M" : ""))
                                .Max(x => x.Month) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Monthly.Value) < 0 ? true : false) : false,

                                StatusUploadDataQ =
                                quarterofyear - _dbContext.Datas.Where(a =>
                                  a.KPILevelCode == kpiLevel.KPILevelCode &&
                                  a.KPIKind == (kpiLevel.QuaterlyChecked == true ? "Q" : ""))
                                .Max(x => x.Quater) > 1 ? false : quarterofyear - _dbContext.Datas.Where(a =>
                                   a.KPILevelCode == kpiLevel.KPILevelCode &&
                                   a.KPIKind == (kpiLevel.QuaterlyChecked == true ? "Q" : ""))
                                .Max(x => x.Quater) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Quaterly.Value) < 0 ? true : false) : false, //true dung han flase tre han

                                StatusUploadDataY =
                                year - _dbContext.Datas.Where(a =>
                                  a.KPILevelCode == kpiLevel.KPILevelCode &&
                                  a.KPIKind == (kpiLevel.YearlyChecked == true ? "Y" : ""))
                                .Max(x => x.Year) > 1 ? false : year - _dbContext.Datas.Where(a =>
                                      a.KPILevelCode == kpiLevel.KPILevelCode &&
                                      a.KPIKind == (kpiLevel.YearlyChecked == true ? "Y" : ""))
                                .Max(x => x.Year) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Yearly.Value) < 0 ? true : false) : false,

                            };



                if (categoryID != 0)
                {
                    model = model.Where(x => x.CategoryID == categoryID);
                }

                int totalRow = model.Count();

                model = model.OrderByDescending(x => x.CreateTime)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize);


                return new
                {
                    data = model,
                    total = totalRow,
                    status = true
                };
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return "";
            }

        }

        /// <summary>
        /// Lấy dữ liệu cho chart js.
        /// </summary>
        /// <param name="kpiid"></param>
        /// <param name="kpilevel"></param>
        /// <param name="period"></param>
        /// <returns>Danh sách value theo thời gian</returns>
        public object ListDatas(string kpilevelcode, string period)
        {
            if (!string.IsNullOrEmpty(kpilevelcode) && !string.IsNullOrEmpty(period))
            {
                //label chartjs
                var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
                var label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name;
                //datasets chartjs
                var model = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode);

                if (period == "W".ToUpper())
                {

                    var datasets = model.Where(x => x.KPIKind == "W").OrderBy(x => x.Week).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.KPIKind == "W").OrderBy(x => x.Week).Select(x => x.Week).ToArray();


                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "M".ToUpper())
                {

                    var datasets = model.Where(x => x.KPIKind == "M").OrderBy(x => x.Month).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.KPIKind == "M").OrderBy(x => x.Month).Select(x => x.Month).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "Q".ToUpper())
                {
                    var datasets = model.Where(x => x.KPIKind == "Q").OrderBy(x => x.Quater).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.KPIKind == "Q").OrderBy(x => x.Quater).Select(x => x.Quater).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "Y".ToUpper())
                {

                    var datasets = model.Where(x => x.KPIKind == "Y").OrderBy(x => x.Year).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.KPIKind == "Y").OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public bool AddComment(EF.Comment entity)
        {
            try
            {

                _dbContext.Comments.Add(entity);
                _dbContext.SaveChanges();
                //var seencmt = new SeenComment();
                //seencmt.CommentID = entity.ID;
                //seencmt.UserID = entity.UserID;
                //seencmt.Status = true;
                //_dbContext.SeenComments.Add(seencmt);
                //_dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool AddCommentHistory(int userid)
        {
            try
            {
                var comments = _dbContext.Comments.ToList();
                foreach (var comment in comments)
                {
                    var item = _dbContext.SeenComments.FirstOrDefault(x => x.UserID == comment.UserID && x.UserID == comment.ID);
                    if (item == null)
                    {
                        var seencmt = new SeenComment();
                        seencmt.CommentID = comment.ID;
                        seencmt.UserID = userid;
                        seencmt.Status = true;
                        _dbContext.SeenComments.Add(seencmt);
                        _dbContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object ListComments(string kpilevelcode)
        {
            //Cat chuoi
            var value = kpilevelcode.ToSafetyString().Split(','); //Chuoi nhan tu client bao gom kpilevelcode, period, userid
            var code = value[0].Substring(0, value[0].Length - 1).ToSafetyString(); //KPILevelCode
            var period = value[0].Substring(value[0].Length - 1, 1).ToUpper().ToSafetyString();
            var userid = value[1].ToInt();

            //lay tat ca comment cua kpi
            var listcmts = _dbContext.Comments.Where(x => x.KPILevelCode == code && x.Period == period).ToList();

            //Tong tat ca cac comment cua kpi
            var totalcomment = listcmts.Count();

            //Tong cmt cua userid da xem kpi
            var totalseencomment = _dbContext.SeenComments.Where(x => x.UserID == userid).Count();

            //Lay ra tat ca cac comment cua kpi(userid nao post comment len thi mac dinh userid do da xem comment cua chinh minh roi)
            var data = _dbContext.Comments.Where(x => x.KPILevelCode == code && x.Period == period)
               .Select(x => new CommentVM
               {
                   CommentID = x.ID,
                   UserID = x.UserID,
                   CommentMsg = x.CommentMsg,
                   KPILevelCode = x.KPILevelCode,
                   CommentedDate = x.CommentedDate,
                   FullName = _dbContext.Users.FirstOrDefault(a => a.ID == x.UserID).FullName,
                   Period = x.Period,
                   Read = true
               })
               .OrderByDescending(x => x.CommentedDate)
               .ToList();

            //
            if (totalcomment > totalseencomment)
            {
                //Neu userid nao ma chua xem comment thi hien thi la chua xem
                data = data.Select(x => new CommentVM
                {
                    CommentID = x.CommentID,
                    UserID = x.UserID,
                    CommentMsg = x.CommentMsg,
                    KPILevelCode = x.KPILevelCode,
                    CommentedDate = x.CommentedDate,
                    FullName = _dbContext.Users.FirstOrDefault(a => a.ID == x.UserID).FullName,
                    Period = x.Period,
                    //read=true la da xem, nguoc lai la chua xem
                    Read = _dbContext.SeenComments.FirstOrDefault(a => a.UserID == userid && a.CommentID == x.CommentID) == null ? false : true
                })
              .OrderByDescending(x => x.CommentedDate)
              .ToList();

                foreach (var item in listcmts)
                {
                    //Neu userid xem comment roi thi luu vao lich su la da xem
                    if (_dbContext.SeenComments.FirstOrDefault(x => x.UserID == userid && x.CommentID == item.ID) == null)
                    {
                        var seencomment = new SeenComment()
                        {
                            CommentID = item.ID,
                            UserID = userid
                        };
                        _dbContext.SeenComments.Add(seencomment);
                        _dbContext.SaveChanges();
                    }

                }
            }

            return new
            {
                data,
                total = _dbContext.Comments.Where(x => x.KPILevelCode == code && x.Period == period).Count()
            };

        }

        public object LoadData(string obj)
        {

            var value = obj.ToSafetyString().Split(',');
            var code = value[0].Substring(0, value[0].Length - 1).ToSafetyString();
            var period = value[0].Substring(value[0].Length - 1, 1).ToUpper().ToSafetyString();
            var userid = value[1].ToInt();
            var data = _dbContext.Comments
               .Where(x => x.KPILevelCode == code && x.Period == period)
               .Select(x => new CommentVM
               {
                   CommentMsg = x.CommentMsg,
                   KPILevelCode = x.KPILevelCode,
                   CommentedDate = x.CommentedDate,
                   FullName = _dbContext.Users.FirstOrDefault(a => a.ID == x.UserID).FullName,
                   Period = x.Period,
                   Read = _dbContext.SeenComments.FirstOrDefault(b => b.CommentID == x.ID && b.UserID == userid).Status
               })
               .OrderByDescending(x => x.CommentedDate)
               .Take(4).ToList();

            return new
            {
                data,
                total = _dbContext.Comments.Where(x => x.KPILevelCode == code && x.Period == period).Count()
            };


        }

        public object LoadDataProvide(string obj)
        {
            object listCompare = null;
            var value = obj.ToSafetyString().Split(',');
            var kpilevelcode = value[0].ToSafetyString();
            var period = value[1].ToSafetyString();

            var itemkpilevel = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
            var itemlevel = _dbContext.Levels.FirstOrDefault(x => x.ID == itemkpilevel.LevelID).LevelNumber;
            var kpiid = itemkpilevel.KPIID;
            //Lay ra tat ca kpiLevel cung levelNumber

            if (period == "W")
            {

                listCompare = _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.WeeklyChecked == true)
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true

                    }).
                    Where(c => c.LevelNumber == itemlevel)
                    .ToList();
            }

            if (period == "M")
            {
                listCompare = _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.MonthlyChecked == true)
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true

                    }).
                    Where(c => c.LevelNumber == itemlevel)
                    .ToList();
            }

            if (period == "Q")
            {
                listCompare = _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.QuaterlyChecked == true)
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true

                    }).
                    Where(c => c.LevelNumber == itemlevel)
                    .ToList();
            }

            if (period == "Y")
            {
                listCompare = _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.YearlyChecked == true)
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true

                    }).
                    Where(c => c.LevelNumber == itemlevel)
                    .ToList();
            }
            //Lay tat ca kpilevel cung period

            return new
            {
                listCompare
            };
        }
    }
}

