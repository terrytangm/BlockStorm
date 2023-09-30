using BlockStorm.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockStorm.EFModels
{
    public partial class BlockchainContext : DbContext
    {
        void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            var cryptConverter = new CryptConverter();
            var bigIntegerConverter = new BigIntegerConverter();
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.PrivateKey).HasConversion(cryptConverter);
                entity.Property(e => e.Mnemonic).HasConversion(cryptConverter);
                entity.Property(e => e.Password).HasConversion(cryptConverter);
                entity.Property(e => e.Path).HasConversion(cryptConverter);
            });

            modelBuilder.Entity<AccountBalance>(entity =>
            {
                entity.Property(e => e.Balance).HasConversion(bigIntegerConverter);
            });
        }

    }

    public class CryptConverter : ValueConverter<string?, string?>
    {
        public CryptConverter()
            : base(
                v => Crypto.RST_AesEncrypt_Base64(v),
                v => Crypto.RST_AesDecrypt_Base64(v)
                )
        { }
    }

    public class BigIntegerConverter : ValueConverter<BigInteger?, string?>
    {
        public BigIntegerConverter()
            : base(
                  v=> v==null? null: v.ToString(),
                  v=> v==null? null:BigInteger.Parse(v)
                  )
        { }
    }
}
