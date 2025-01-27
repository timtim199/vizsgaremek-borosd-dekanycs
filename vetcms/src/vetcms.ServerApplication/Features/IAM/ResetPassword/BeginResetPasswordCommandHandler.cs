﻿using Azure.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Features.IAM;
using Microsoft.Extensions.Configuration;

namespace vetcms.ServerApplication.Features.IAM.ResetPassword
{
    internal class BeginResetPasswordCommandHandler(IUserRepository userRepository, IMailService mailService, IConfiguration config) : IRequestHandler<BeginResetPasswordApiCommand, BeginResetPasswordApiCommandResponse>
    {
        public async Task<BeginResetPasswordApiCommandResponse> Handle(BeginResetPasswordApiCommand request, CancellationToken cancellationToken)
        {
            if(userRepository.HasUserByEmail(request.Email))
            {
                return await ProcessRequest(request);
            }
            else
            {
                return new BeginResetPasswordApiCommandResponse(false, "Nincs ilyen felhasználó.");
            }
            throw new NotImplementedException();
        }

        private async Task<BeginResetPasswordApiCommandResponse> ProcessRequest(BeginResetPasswordApiCommand request)
        {
            PasswordReset resetPasswordEntity = await CreatePasswordResetEntity(request);
            int id = await SendResetEmail(request, resetPasswordEntity);

            if(config.GetValue<bool>("MailServices:UseOnlyLocal"))
            {
                return new BeginResetPasswordApiCommandResponse(true)
                {
                    Message = $"[BEMUTATÓ MÓD] Az email elküldése sikeres volt. A bemutató céljából az alábbi linken nyitható meg: {mailService.GetEmailPreviewRoute(id)}"
                };

            }
            return new BeginResetPasswordApiCommandResponse(true);
        }

        private async Task<PasswordReset> CreatePasswordResetEntity(BeginResetPasswordApiCommand request)
        {
            string token = GenerateToken();
            PasswordReset resetPasswordEntity = new PasswordReset();
            resetPasswordEntity.Email = request.Email;
            resetPasswordEntity.Code = token;
            User user = userRepository.GetByEmail(request.Email);

            user.PasswordResets.Add(resetPasswordEntity);

            await userRepository.UpdateAsync(user);

            return resetPasswordEntity;
        }

        private async Task<int> SendResetEmail(BeginResetPasswordApiCommand request, PasswordReset resetPasswordEntity)
        {
            User user = userRepository.GetByEmail(request.Email);
            resetPasswordEntity.User = user;
            return await mailService.SendPasswordResetEmailAsync(resetPasswordEntity);
        }

        private string GenerateToken()
            => Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
    }
}
