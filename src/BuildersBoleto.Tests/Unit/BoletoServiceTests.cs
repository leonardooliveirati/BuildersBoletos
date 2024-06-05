using BuildersBoleto.Domain.Entities;
using BuildersBoleto.Domain.Intefaces.Proxy;
using BuildersBoleto.Domain.Intefaces.Repositories;
using BuildersBoleto.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildersBoleto.Tests.Unit
{
    public class BoletoServiceTests
    {
        [Fact]
        public async Task CalcularValorBoletoAsync_ValidBarcodeAndPaymentDate_ReturnsCorrectValues()
        {
            var token = "valid_token";
            var barcode = "34191790010104351004791020150008291070026000";
            var paymentDate = new DateTime(2024, 06, 05);
            var mockBoleto = new BoletoModel
            {
                Type = "NPC",
                DueDate = new DateTime(2024, 06, 03),
                Amount = 250.0m
            };
            var mockApiClient = new Mock<IBoletoApiClient>();
            mockApiClient.Setup(client => client.GetBoletoAsync(barcode, token)).ReturnsAsync(mockBoleto);

            var mockRepository = new Mock<IBoletoRepository>();
            var service = new BoletoService(mockRepository.Object, mockApiClient.Object);

            var result = await service.CalcularValorBoletoAsync(barcode, paymentDate, token);

            Assert.Equal(mockBoleto.Amount + 0.066m * mockBoleto.Amount + 0.02m * mockBoleto.Amount, result.Amount);
            Assert.Equal(mockBoleto.Amount, result.OriginalAmount);
            Assert.Equal("2024-06-03", result.DueDate);
            Assert.Equal("2024-06-05", result.PaymentDate);
            Assert.Equal(0.066m * mockBoleto.Amount, result.InterestAmountCalculated);
            Assert.Equal(0.02m * mockBoleto.Amount, result.FineAmountCalculated);
        }

        [Fact]
        public async Task CalcularValorBoletoAsync_InvalidBoletoType_ThrowsArgumentException()
        {
            var token = "valid_token";
            var barcode = "valid_barcode";
            var paymentDate = new DateTime(2024, 06, 05);
            var mockBoleto = new BoletoModel { Type = "INVALID_TYPE" };
            var mockApiClient = new Mock<IBoletoApiClient>();
            mockApiClient.Setup(client => client.GetBoletoAsync(barcode, token)).ReturnsAsync(mockBoleto);

            var mockRepository = new Mock<IBoletoRepository>();
            var service = new BoletoService(mockRepository.Object, mockApiClient.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CalcularValorBoletoAsync(barcode, paymentDate, token));
        }

        [Fact]
        public async Task CalcularValorBoletoAsync_NotLatePayment_ThrowsArgumentException()
        {
            var token = "valid_token";
            var barcode = "valid_barcode";
            var paymentDate = new DateTime(2024, 06, 03);
            var mockBoleto = new BoletoModel { Type = "NPC", DueDate = paymentDate.AddDays(1) };
            var mockApiClient = new Mock<IBoletoApiClient>();
            mockApiClient.Setup(client => client.GetBoletoAsync(barcode, token)).ReturnsAsync(mockBoleto);

            var mockRepository = new Mock<IBoletoRepository>();
            var service = new BoletoService(mockRepository.Object, mockApiClient.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => service.CalcularValorBoletoAsync(barcode, paymentDate, token));
        }
    }
}
