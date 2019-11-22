using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prensaestudiantil.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context, 
            IUserHelper userHelper
            )
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckRole("Manager");
            await CheckRole("Writer");
            await CheckUserAsync("Sevann", "Radhak", "sevann.radhak@gmail.com", "54 9 11 73627795");
            await CheckUserAsync("Prensa", "Estudiantil", "prensaestudiantil@hotmail.com", "54 9 11 73627795");
            await CheckPublicationCategoriesAsync();
            await CheckPublicationnsAsync();
            await CheckYoutubeVideosAsync();
        }

        private async Task CheckRole(string roleName)
        {
            await _userHelper.CheckRoleAsync(roleName);
        }

        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phoneNumber)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    IsEnabled = true,
                    PhoneNumber = phoneNumber,
                    UserName = email
                };
                // TODO: Change password and hardcode user data by configuration
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, "Manager");
                await _userHelper.AddUserToRoleAsync(user, "Writer");

                // Token confirmation
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }

        private async Task CheckPublicationCategoriesAsync()
        {
            if (!_context.PublicationCategories.Any())
            {
                PublicationCategory pc1 = new PublicationCategory { Name = "Estudiantil" };
                PublicationCategory pc2 = new PublicationCategory { Name = "Opinión" };
                PublicationCategory pc3 = new PublicationCategory { Name = "Social" };

                _context.PublicationCategories.Add(pc1);
                _context.PublicationCategories.Add(pc2);
                _context.PublicationCategories.Add(pc3);
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Check if there is not Publication. Create two (one for Opinion categories)
        /// </summary>
        /// <returns></returns>
        private async Task CheckPublicationnsAsync()
        {
            if (!_context.Publications.Any())
            {
                User user = _context.Users.FirstOrDefault();
                PublicationCategory pc = _context.PublicationCategories.FirstOrDefault();
                PublicationCategory pOpinion = _context.PublicationCategories.Where(p => p.Name == "Opinión").FirstOrDefault();

                if (pOpinion == null)
                {
                    _context.PublicationCategories.Add(new PublicationCategory { Name = "Opinión" });
                    pOpinion = _context.PublicationCategories.Where(p => p.Name == "Opinión").FirstOrDefault();
                }

                Publication publication1 = new Publication
                {
                    // TODO: Verify if need an initial image and configure it
                    Body = "La investigación fue mandatada por la Unidad de Trauma Ocular del Hospital del Salvador, " +
                        "entidad que ha recibido a muchos de los heridos en las protestas." +
                        "En el estudio del Departamento de Ingeniería Mecánica de la Facultad de Ciencias Físicas y Matemáticas de la " +
                        "Universidad de Chile se detalla que en la estructura de estos balines hay solo un 20 por ciento de caucho o goma, " +
                        "situación que sería una de las causantes de la gravedad de las heridas oculares. " +
                        "" +
                        "Para la muestra se utilizaron dos balines obtenidos 'de pacientes afectados por impacto de dichos proyectiles " +
                        "durante las manifestaciones' y en el estudio se revisaron documentos de Carabineros que detallan la composición y " +
                        "la forma de utilización de este tipo de munición.",
                    Date = DateTime.Now.ToUniversalTime(),
                    Footer = "Tomado de The Clinic (Chile)",
                    Header = "En medio de la condena social a Carabineros a causa de los más de 200 heridos con traumas oculares en casi " +
                    "un mes de manifestaciones, hoy un estudio acreditó la composición de los balines que utiliza la policía.",
                    PublicationCategory = pc,
                    User = user,
                    Title = "Perdigones utilizados por carabineros contra manifestantes contienen solo un 20 " +
                        "por ciento de goma según estudio"
                };

                Publication publication2 = new Publication
                {
                    // TODO: Verify if need an initial image and configure it
                    Author = "Brodsky",
                    Body = "La investigación fue mandatada por la Unidad de Trauma Ocular del Hospital del Salvador, " +
                        "entidad que ha recibido a muchos de los heridos en las protestas." +
                        "En el estudio del Departamento de Ingeniería Mecánica de la Facultad de Ciencias Físicas y Matemáticas de la " +
                        "Universidad de Chile se detalla que en la estructura de estos balines hay solo un 20 por ciento de caucho o goma, " +
                        "situación que sería una de las causantes de la gravedad de las heridas oculares. " +
                        "" +
                        "Para la muestra se utilizaron dos balines obtenidos 'de pacientes afectados por impacto de dichos proyectiles " +
                        "durante las manifestaciones' y en el estudio se revisaron documentos de Carabineros que detallan la composición y " +
                        "la forma de utilización de este tipo de munición.",
                    Date = DateTime.Now.ToUniversalTime(),
                    Footer = "Tomado de The Clinic (Chile)",
                    Header = "Hago memoria para no abundar en el ruido y el humo, considerando que una nueva temporada de odiosidad " +
                    "mutua se ha declarado en el país. Me interesa en particular el campo cultural donde trabajo, que es el de los " +
                    "escritores y escritoras, poetas y críticos, intelectuales y columnistas.",
                    PublicationCategory = pOpinion,
                    User = user,
                    Title = "Cancha rayada: Sobre el odio, la funa y el opio de los intelectuales"
                };

                _context.Publications.Add(publication1);
                _context.Publications.Add(publication2);

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckYoutubeVideosAsync()
        {
            if (!_context.YoutubeVideos.Any())
            {
                User user = _context.Users.FirstOrDefault();

                _context.YoutubeVideos.Add(new YoutubeVideo
                {
                    Name = "Cuidar el campus es tarea de todos #ViveLaUIS",
                    URL = "0rr6bQkPzFY",
                    User = user
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
