using System.Text;

namespace ITLANetworking.Core.Application.Helpers
{
    public static class EmailTemplates
    {
        #region Base Template Structure

        private static string GetBaseEmailTemplate(string title, string headerColor, string headerIcon, string headerTitle, string headerSubtitle, string content)
        {
            return $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            background: {headerColor};
            padding: 20px;
        }}
        
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 20px;
            overflow: hidden;
            box-shadow: 0 20px 40px rgba(0,0,0,0.1);
        }}
        
        .header {{
            background: {headerColor};
            padding: 40px 30px;
            text-align: center;
            color: white;
        }}
        
        .header-icon {{
            width: 80px;
            height: 80px;
            background: rgba(255,255,255,0.2);
            border-radius: 20px;
            margin: 0 auto 20px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 32px;
        }}
        
        .success-icon {{
            width: 100px;
            height: 100px;
            background: rgba(255,255,255,0.2);
            border-radius: 50%;
            margin: 0 auto 20px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 48px;
            animation: bounce 2s infinite;
        }}
        
        @keyframes bounce {{
            0%, 20%, 50%, 80%, 100% {{
                transform: translateY(0);
            }}
            40% {{
                transform: translateY(-10px);
            }}
            60% {{
                transform: translateY(-5px);
            }}
        }}
        
        .header h1 {{
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 10px;
        }}
        
        .header p {{
            font-size: 16px;
            opacity: 0.9;
        }}
        
        .content {{
            padding: 40px 30px;
        }}
        
        .welcome-message, .success-message {{
            text-align: center;
            margin-bottom: 30px;
        }}
        
        .welcome-message h2, .success-message h2 {{
            color: #333;
            font-size: 24px;
            margin-bottom: 15px;
        }}
        
        .welcome-message p, .success-message p {{
            color: #666;
            font-size: 16px;
            line-height: 1.6;
        }}
        
        .cta-section {{
            text-align: center;
            margin: 40px 0;
        }}
        
        .cta-button {{
            display: inline-block;
            background: {headerColor};
            color: white;
            text-decoration: none;
            padding: 16px 32px;
            border-radius: 50px;
            font-weight: 600;
            font-size: 16px;
            transition: transform 0.3s ease;
            box-shadow: 0 10px 20px rgba(102, 126, 234, 0.3);
        }}
        
        .cta-button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 15px 30px rgba(102, 126, 234, 0.4);
        }}
        
        .alert-box {{
            background: #fef3c7;
            border: 1px solid #f59e0b;
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 30px;
        }}
        
        .alert-box .icon {{
            font-size: 24px;
            margin-bottom: 10px;
        }}
        
        .info-box {{
            background: #f0fdf4;
            border: 1px solid #10b981;
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 30px;
        }}
        
        .info-box .icon {{
            font-size: 48px;
            margin-bottom: 15px;
        }}
        
        .backup-link {{
            background: #f0f0f0;
            padding: 20px;
            border-radius: 10px;
            margin: 20px 0;
            word-break: break-all;
        }}
        
        .backup-link p {{
            color: #666;
            font-size: 12px;
            margin-bottom: 10px;
        }}
        
        .backup-link a {{
            color: #667eea;
            text-decoration: none;
            font-size: 12px;
        }}
        
        .footer {{
            background: #f8f9ff;
            padding: 30px;
            text-align: center;
            border-top: 1px solid #e0e6ff;
        }}
        
        .footer p {{
            color: #666;
            font-size: 14px;
            margin-bottom: 10px;
        }}
        
        .timestamp {{
            background: #f8fafc;
            padding: 15px;
            border-radius: 10px;
            margin: 20px 0;
            font-family: monospace;
            font-size: 14px;
            color: #6b7280;
            text-align: center;
        }}
        
        @media (max-width: 600px) {{
            .container {{
                margin: 10px;
                border-radius: 15px;
            }}
            
            .header, .content, .footer {{
                padding: 20px;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='{(headerIcon.Contains("✅") ? "success-icon" : "header-icon")}'>
                {headerIcon}
            </div>
            <h1>{headerTitle}</h1>
            <p>{headerSubtitle}</p>
        </div>
        
        <div class='content'>
            {content}
        </div>
        
        <div class='footer'>
            <p style='margin-top: 20px; font-size: 12px; color: #999;'>
                © {DateTime.Now.Year} ITLA Networking. Todos los derechos reservados.<br>
                Este correo fue generado automáticamente, por favor no respondas.
            </p>
        </div>
    </div>
</body>
</html>";
        }

        #endregion

        #region Welcome Email Template

        public static string GetWelcomeEmail(string firstName, string verificationUri)
        {
            var content = $@"
            <div class='welcome-message'>
                <h2>¡Hola {firstName}! 👋</h2>
                <p>¡Bienvenido a ITLA Networking! Estamos emocionados de tenerte en nuestra comunidad estudiantil. Para comenzar a conectar con tus compañeros, necesitas confirmar tu dirección de correo electrónico.</p>
            </div>
            
            <div class='cta-section'>
                <a href='{verificationUri}' class='cta-button'>
                    ✅ Confirmar mi cuenta
                </a>
            </div>
            
            {GetPlatformFeatures()}
            
            <div class='backup-link'>
                <p>Si el botón no funciona, copia y pega este enlace en tu navegador:</p>
                <a href='{verificationUri}'>{verificationUri}</a>
            </div>
            
            {GetHelpSection()}";

            return GetBaseEmailTemplate(
                "Bienvenido a ITLA Networking",
                "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
                "👥",
                "ITLA Networking",
                "Conecta con tus compañeros de estudio",
                content
            );
        }

        private static string GetPlatformFeatures()
        {
            return @"
            <div style='display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; margin: 40px 0;'>
                <div style='text-align: center; padding: 20px; background: #f8f9ff; border-radius: 15px;'>
                    <div style='width: 50px; height: 50px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 12px; margin: 0 auto 15px; display: flex; align-items: center; justify-content: center; color: white; font-size: 20px;'>👥</div>
                    <h3 style='color: #333; font-size: 18px; margin-bottom: 10px;'>Conecta con Amigos</h3>
                    <p style='color: #666; font-size: 14px;'>Encuentra y conecta with tus compañeros de clase y carrera</p>
                </div>
                <div style='text-align: center; padding: 20px; background: #f8f9ff; border-radius: 15px;'>
                    <div style='width: 50px; height: 50px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 12px; margin: 0 auto 15px; display: flex; align-items: center; justify-content: center; color: white; font-size: 20px;'>💬</div>
                    <h3 style='color: #333; font-size: 18px; margin-bottom: 10px;'>Comparte Experiencias</h3>
                    <p style='color: #666; font-size: 14px;'>Publica contenido, comenta y reacciona a las publicaciones</p>
                </div>
                <div style='text-align: center; padding: 20px; background: #f8f9ff; border-radius: 15px;'>
                    <div style='width: 50px; height: 50px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 12px; margin: 0 auto 15px; display: flex; align-items: center; justify-content: center; color: white; font-size: 20px;'>🚢</div>
                    <h3 style='color: #333; font-size: 18px; margin-bottom: 10px;'>Juega Battleship</h3>
                    <p style='color: #666; font-size: 14px;'>Diviértete con el juego de batalla naval integrado</p>
                </div>
            </div>";
        }

        private static string GetHelpSection()
        {
            return @"
            <div style='text-align: center; margin-top: 30px;'>
                <p><strong>¿Necesitas ayuda?</strong></p>
                <p>Si tienes problemas para confirmar tu cuenta, contacta a nuestro equipo de soporte.</p>
                
                <div style='margin: 20px 0;'>
                    <a href='#' style='display: inline-block; width: 40px; height: 40px; background: #667eea; color: white; text-decoration: none; border-radius: 50%; margin: 0 5px; line-height: 40px; transition: background 0.3s ease;'>📧</a>
                    <a href='#' style='display: inline-block; width: 40px; height: 40px; background: #667eea; color: white; text-decoration: none; border-radius: 50%; margin: 0 5px; line-height: 40px; transition: background 0.3s ease;'>📱</a>
                    <a href='#' style='display: inline-block; width: 40px; height: 40px; background: #667eea; color: white; text-decoration: none; border-radius: 50%; margin: 0 5px; line-height: 40px; transition: background 0.3s ease;'>🌐</a>
                </div>
            </div>";
        }

        #endregion

        #region Account Activated Email Template

        public static string GetAccountActivatedEmail(string firstName)
        {
            var content = $@"
            <div class='success-message' style='text-align: center;'>
                <h2 style='color: #10b981;'>¡Perfecto, {firstName}! 🎉</h2>
                <p style='margin-bottom: 30px;'>Tu cuenta de ITLA Networking ha sido activada exitosamente. Ahora puedes acceder a todas las funcionalidades de nuestra plataforma y comenzar a conectar con tus compañeros.</p>
                
                <a href='#' class='cta-button' style='background: linear-gradient(135deg, #10b981 0%, #059669 100%); box-shadow: 0 10px 20px rgba(16, 185, 129, 0.3);'>🚀 Iniciar Sesión Ahora</a>
            </div>
            
            {GetNextStepsSection()}";

            return GetBaseEmailTemplate(
                "Cuenta Activada - ITLA Networking",
                "linear-gradient(135deg, #10b981 0%, #059669 100%)",
                "✅",
                "¡Cuenta Activada!",
                "Tu cuenta ha sido confirmada exitosamente",
                content
            );
        }

        private static string GetNextStepsSection()
        {
            return @"
            <div style='background: #f0fdf4; padding: 30px; border-radius: 15px; margin: 30px 0;'>
                <h3 style='color: #059669; font-size: 20px; margin-bottom: 20px; text-align: center;'>🎯 Próximos pasos recomendados:</h3>
                
                <div style='display: flex; align-items: center; margin-bottom: 15px; padding: 15px; background: white; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.05);'>
                    <div style='width: 30px; height: 30px; background: #10b981; color: white; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-weight: bold; margin-right: 15px;'>1</div>
                    <div>
                        <strong>Completa tu perfil</strong><br>
                        <small>Agrega una foto y actualiza tu información personal</small>
                    </div>
                </div>
                
                <div style='display: flex; align-items: center; margin-bottom: 15px; padding: 15px; background: white; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.05);'>
                    <div style='width: 30px; height: 30px; background: #10b981; color: white; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-weight: bold; margin-right: 15px;'>2</div>
                    <div>
                        <strong>Busca amigos</strong><br>
                        <small>Encuentra y conecta con tus compañeros de clase</small>
                    </div>
                </div>
                
                <div style='display: flex; align-items: center; margin-bottom: 15px; padding: 15px; background: white; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.05);'>
                    <div style='width: 30px; height: 30px; background: #10b981; color: white; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-weight: bold; margin-right: 15px;'>3</div>
                    <div>
                        <strong>Crea tu primera publicación</strong><br>
                        <small>Comparte algo interesante con la comunidad</small>
                    </div>
                </div>
                
                <div style='display: flex; align-items: center; padding: 15px; background: white; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.05);'>
                    <div style='width: 30px; height: 30px; background: #10b981; color: white; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-weight: bold; margin-right: 15px;'>4</div>
                    <div>
                        <strong>Juega Battleship</strong><br>
                        <small>Desafía a tus amigos en una batalla naval</small>
                    </div>
                </div>
            </div>
            
            <div style='text-align: center; margin-top: 30px;'>
                <p><strong>¡Bienvenido a la comunidad ITLA! 🎓</strong></p>
            </div>";
        }

        #endregion

        #region Password Reset Email Template

        public static string GetPasswordResetEmail(string firstName, string resetUri)
        {
            var content = $@"
            <div class='alert-box'>
                <div style='font-size: 24px; margin-bottom: 10px;'>⚠️</div>
                <h3>Solicitud de Recuperación de Contraseña</h3>
                <p>Hola <strong>{firstName}</strong>, hemos recibido una solicitud para restablecer la contraseña de tu cuenta en ITLA Networking.</p>
            </div>
            
            <div style='text-align: center; margin: 30px 0;'>
                <p style='margin-bottom: 20px;'>Si fuiste tú quien solicitó este cambio, haz clic en el siguiente botón:</p>
                <a href='{resetUri}' class='cta-button' style='background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); box-shadow: 0 10px 20px rgba(245, 158, 11, 0.3);'>🔑 Restablecer Contraseña</a>
            </div>
            
            {GetExpiryNotice()}
            {GetSecurityTips()}
            
            <div class='backup-link'>
                <p>Si el botón no funciona, copia y pega este enlace:</p>
                <a href='{resetUri}'>{resetUri}</a>
            </div>
            
            {GetSecurityFooter()}";

            return GetBaseEmailTemplate(
                "Recuperar Contraseña - ITLA Networking",
                "linear-gradient(135deg, #f59e0b 0%, #d97706 100%)",
                "🔐",
                "Recuperar Contraseña",
                "Solicitud de restablecimiento de contraseña",
                content
            );
        }

        private static string GetExpiryNotice()
        {
            return @"
            <div style='background: #fee2e2; border: 1px solid #fca5a5; border-radius: 10px; padding: 15px; margin: 20px 0; text-align: center;'>
                <div style='font-size: 20px; margin-bottom: 5px;'>⏰</div>
                <p><strong>Importante:</strong> Este enlace expirará en 24 horas por seguridad.</p>
            </div>";
        }

        private static string GetSecurityTips()
        {
            return @"
            <div style='background: #f8fafc; padding: 25px; border-radius: 15px; margin: 30px 0;'>
                <h3 style='color: #374151; margin-bottom: 15px;'>🛡️ Consejos de Seguridad:</h3>
                <ul style='list-style: none; padding: 0;'>
                    <li style='padding: 8px 0; border-bottom: 1px solid #e5e7eb;'>✅ Si no solicitaste este cambio, ignora este correo</li>
                    <li style='padding: 8px 0; border-bottom: 1px solid #e5e7eb;'>✅ Nunca compartas tu contraseña con nadie</li>
                    <li style='padding: 8px 0; border-bottom: 1px solid #e5e7eb;'>✅ Usa una contraseña fuerte y única</li>
                    <li style='padding: 8px 0;'>✅ Considera usar un administrador de contraseñas</li>
                </ul>
            </div>";
        }

        private static string GetSecurityFooter()
        {
            return @"
            <div style='text-align: center; margin-top: 30px;'>
                <p><strong>¿No solicitaste este cambio?</strong></p>
                <p>Si no fuiste tú, puedes ignorar este correo de forma segura. Tu cuenta permanece protegida.</p>
            </div>";
        }

        #endregion

        #region Password Changed Email Template

        public static string GetPasswordChangedEmail(string firstName)
        {
            var content = $@"
            <div class='info-box' style='text-align: center;'>
                <div style='font-size: 48px; margin-bottom: 15px;'>✅</div>
                <h2>¡Listo, {firstName}!</h2>
                <p>Tu contraseña ha sido restablecida exitosamente. Tu cuenta está ahora activa y puedes iniciar sesión con tu nueva contraseña.</p>
            </div>
            
            <div class='timestamp'>
                <strong>Fecha y hora del cambio:</strong><br>
                {DateTime.Now:dddd, dd MMMM yyyy 'a las' HH:mm:ss}
            </div>
            
            <div style='text-align: center; margin: 30px 0;'>
                <a href='#' class='cta-button' style='background: linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%); box-shadow: 0 10px 20px rgba(139, 92, 246, 0.3);'>🚀 Iniciar Sesión</a>
            </div>
            
            {GetPasswordSecurityInfo()}
            {GetSecurityAdvice()}";

            return GetBaseEmailTemplate(
                "Contraseña Actualizada - ITLA Networking",
                "linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)",
                "🛡️",
                "Contraseña Actualizada",
                "Tu contraseña ha sido cambiada exitosamente",
                content
            );
        }

        private static string GetPasswordSecurityInfo()
        {
            return @"
            <div style='background: #fef3c7; border: 1px solid #f59e0b; border-radius: 10px; padding: 20px; margin: 30px 0;'>
                <h3>🔒 Información de Seguridad</h3>
                <p><strong>¿No fuiste tú?</strong></p>
                <p>Si no cambiaste tu contraseña, contacta inmediatamente a nuestro equipo de soporte. Tu cuenta podría estar comprometida.</p>
            </div>";
        }

        private static string GetSecurityAdvice()
        {
            return @"
            <div style='background: #f0fdf4; padding: 20px; border-radius: 10px; margin: 20px 0;'>
                <h4 style='color: #059669; margin-bottom: 10px;'>💡 Consejos para mantener tu cuenta segura:</h4>
                <ul style='text-align: left; color: #374151;'>
                    <li>No compartas tu contraseña con nadie</li>
                    <li>Usa contraseñas únicas para cada servicio</li>
                    <li>Cierra sesión en dispositivos públicos</li>
                    <li>Mantén actualizada tu información de contacto</li>
                </ul>
            </div>
            
            <div style='text-align: center; margin-top: 30px;'>
                <p><strong>¿Necesitas ayuda?</strong></p>
                <p>Si tienes problemas para acceder a tu cuenta, contacta a nuestro equipo de soporte.</p>
            </div>";
        }

        #endregion

        #region Battleship Game Notifications

        public static string GetBattleshipChallengeEmail(string playerName, string challengerName, string gameUrl)
        {
            var content = $@"
            <div class='welcome-message'>
                <h2>¡{challengerName} te ha desafiado! ⚓</h2>
                <p>¡Hola <strong>{playerName}</strong>! Tu amigo <strong>{challengerName}</strong> te ha desafiado a una épica batalla naval en ITLA Networking. ¿Estás listo para demostrar tus habilidades estratégicas?</p>
            </div>
            
            <div class='cta-section'>
                <a href='{gameUrl}' class='cta-button' style='background: linear-gradient(135deg, #dc2626 0%, #b91c1c 100%); box-shadow: 0 10px 20px rgba(220, 38, 38, 0.3);'>
                    🚢 Aceptar Desafío
                </a>
            </div>
            
            {GetBattleshipGameInfo()}";

            return GetBaseEmailTemplate(
                "Desafío de Battleship - ITLA Networking",
                "linear-gradient(135deg, #dc2626 0%, #b91c1c 100%)",
                "⚓",
                "¡Desafío de Battleship!",
                "Alguien quiere jugar contigo",
                content
            );
        }

        public static string GetBattleshipVictoryEmail(string winnerName, string loserName, string gameResultUrl)
        {
            var content = $@"
            <div class='info-box' style='text-align: center;'>
                <div style='font-size: 48px; margin-bottom: 15px;'>🏆</div>
                <h2>¡Felicidades, {winnerName}!</h2>
                <p>¡Has ganado la batalla naval contra <strong>{loserName}</strong>! Tu estrategia y habilidad te han llevado a la victoria. ¡Bien hecho, almirante!</p>
            </div>
            
            <div style='text-align: center; margin: 30px 0;'>
                <a href='{gameResultUrl}' class='cta-button' style='background: linear-gradient(135deg, #10b981 0%, #059669 100%); box-shadow: 0 10px 20px rgba(16, 185, 129, 0.3);'>🎯 Ver Resultado Detallado</a>
            </div>
            
            {GetVictoryStats()}";

            return GetBaseEmailTemplate(
                "¡Victoria en Battleship! - ITLA Networking",
                "linear-gradient(135deg, #10b981 0%, #059669 100%)",
                "🏆",
                "¡Victoria Naval!",
                "Has ganado la batalla",
                content
            );
        }

        private static string GetBattleshipGameInfo()
        {
            return @"
            <div style='background: #fef2f2; padding: 25px; border-radius: 15px; margin: 30px 0;'>
                <h3 style='color: #dc2626; margin-bottom: 15px; text-align: center;'>🎮 Sobre el Juego:</h3>
                <ul style='list-style: none; padding: 0; text-align: left;'>
                    <li style='padding: 8px 0; border-bottom: 1px solid #fecaca;'>⚓ Coloca estratégicamente tus barcos</li>
                    <li style='padding: 8px 0; border-bottom: 1px solid #fecaca;'>🎯 Ataca las posiciones enemigas</li>
                    <li style='padding: 8px 0; border-bottom: 1px solid #fecaca;'>💥 Hunde toda la flota rival</li>
                    <li style='padding: 8px 0;'>🏆 ¡Conviértete en el almirante supremo!</li>
                </ul>
            </div>";
        }

        private static string GetVictoryStats()
        {
            return @"
            <div style='background: #f0fdf4; padding: 25px; border-radius: 15px; margin: 30px 0;'>
                <h3 style='color: #059669; margin-bottom: 15px; text-align: center;'>📊 Logros Desbloqueados:</h3>
                <div style='display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 15px;'>
                    <div style='text-align: center; padding: 15px; background: white; border-radius: 10px;'>
                        <div style='font-size: 24px; margin-bottom: 5px;'>🎯</div>
                        <strong>Estratega Naval</strong>
                    </div>
                    <div style='text-align: center; padding: 15px; background: white; border-radius: 10px;'>
                        <div style='font-size: 24px; margin-bottom: 5px;'>⚓</div>
                        <strong>Almirante</strong>
                    </div>
                    <div style='text-align: center; padding: 15px; background: white; border-radius: 10px;'>
                        <div style='font-size: 24px; margin-bottom: 5px;'>🏆</div>
                        <strong>Conquistador</strong>
                    </div>
                </div>
            </div>";
        }

        #endregion

        #region Friend Request Notifications

        public static string GetFriendRequestEmail(string recipientName, string senderName, string senderProfileUrl, string acceptUrl)
        {
            var content = $@"
            <div class='welcome-message'>
                <h2>¡Nueva solicitud de amistad! 👥</h2>
                <p>¡Hola <strong>{recipientName}</strong>! <strong>{senderName}</strong> quiere conectar contigo en ITLA Networking. ¡Expande tu red de compañeros y haz nuevas conexiones!</p>
            </div>
            
            <div style='background: #f8f9ff; padding: 25px; border-radius: 15px; margin: 30px 0; text-align: center;'>
                <div style='width: 80px; height: 80px; background: #667eea; border-radius: 50%; margin: 0 auto 15px; display: flex; align-items: center; justify-content: center; color: white; font-size: 32px;'>👤</div>
                <h3 style='color: #333; margin-bottom: 10px;'>{senderName}</h3>
                <p style='color: #666; margin-bottom: 20px;'>Quiere ser tu amigo en ITLA Networking</p>
                <a href='{senderProfileUrl}' style='color: #667eea; text-decoration: none; font-size: 14px;'>Ver perfil completo →</a>
            </div>
            
            <div class='cta-section'>
                <a href='{acceptUrl}' class='cta-button' style='background: linear-gradient(135deg, #10b981 0%, #059669 100%); box-shadow: 0 10px 20px rgba(16, 185, 129, 0.3);'>
                    ✅ Aceptar Solicitud
                </a>
            </div>";

            return GetBaseEmailTemplate(
                "Nueva Solicitud de Amistad - ITLA Networking",
                "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
                "👥",
                "¡Nueva Solicitud!",
                "Alguien quiere ser tu amigo",
                content
            );
        }

        #endregion
    }
}
