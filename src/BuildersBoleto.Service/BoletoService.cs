﻿using BuildersBoleto.Domain.Entities;
using BuildersBoleto.Domain.Intefaces.Proxy;
using BuildersBoleto.Domain.Intefaces.Repositories;
using BuildersBoleto.Domain.Intefaces.Services;
using BuildersBoleto.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildersBoleto.Service
{
    public class BoletoService : IBoletoService
    {
        private readonly IBoletoRepository _boletoRepository;
        private readonly IBoletoApiClient _apiClient;

        public BoletoService(IBoletoRepository boletoRepository, IBoletoApiClient apiClient)
        {
            _boletoRepository = boletoRepository;
            _apiClient = apiClient;
        }
        public async Task<BoletoResponse> CalcularValorBoletoAsync(string barCode, DateTime paymentDate, string token)
        {
            var boleto = await _apiClient.GetBoletoAsync(barCode, token);

            if (boleto.Type != "NPC")
                throw new ArgumentException("Apenas boletos do tipo NPC podem ser calculados.");

            if (boleto.DueDate >= paymentDate)
                throw new ArgumentException("O boleto não está vencido.");

            int daysLate = (paymentDate - boleto.DueDate).Days;
            decimal interest = daysLate * 0.033m * boleto.Amount;
            decimal fine = 0.02m * boleto.Amount;
            decimal amount = boleto.Amount + interest + fine;

            var result = new BoletoResponse
            {
                OriginalAmount = boleto.Amount,
                Amount = amount,
                DueDate = boleto.DueDate.ToString("yyyy-MM-dd"),
                PaymentDate = paymentDate.ToString("yyyy-MM-dd"),
                InterestAmountCalculated = interest,
                FineAmountCalculated = fine
            };

            await _boletoRepository.Salvar(new BoletoEntity()
            {
                Amount = amount,
                DueDate = boleto.DueDate,
                PaymentDate = paymentDate,
                BarCode = barCode,
                FineAmountCalculated = fine,
                InterestAmountCalculated = interest,
                OriginalAmount = boleto.Amount,
            });

            return result;
        }
    }
}
