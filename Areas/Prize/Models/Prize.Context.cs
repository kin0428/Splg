﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Splg.Areas.Prize.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PrizeEntities : DbContext
    {
        public PrizeEntities()
            : base("name=PrizeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<EntryHistories> EntryHistories { get; set; }
        public virtual DbSet<GoodsSpec> GoodsSpec { get; set; }
        public virtual DbSet<Rallies> Rallies { get; set; }
        public virtual DbSet<RallyGoods> RallyGoods { get; set; }
        public virtual DbSet<RallyGoodsRemarks> RallyGoodsRemarks { get; set; }
        public virtual DbSet<RallyGoodsRemarksLink> RallyGoodsRemarksLink { get; set; }
        public virtual DbSet<RallyGoodsRemarksText> RallyGoodsRemarksText { get; set; }
    }
}
