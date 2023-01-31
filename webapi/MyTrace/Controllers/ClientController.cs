using Microsoft.AspNetCore.Mvc.ModelBinding;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrace.Models;
using MyTrace.Utils;
using NBitcoin.Secp256k1;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using System.Globalization;
using System.Runtime.InteropServices;
using MyTrace.Domain;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {

        /**<summary> 
        * Context of database
        * </summary>
        */
        private readonly MyTraceContext _context;
        private readonly UserData _userGetter;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;

        /**<summary> 
        * ComponentController Builder 
        * </summary>
        */
        public ClientController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;   
        }

        /**<summary> 
         * Function that returns all valid clients from a organization
         * </summary>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getClients")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<Client>>> GetClients([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = user.OrganizationId;

            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new LotArgumentException("Organization Not Found!");
            }

            List<Client> clients;

            clients = await _context.Clients
                        .Where(p => p.OrganizationId == idOrg && p.DeletedAt == null)
                        .Where(p => search == null || (p.Email.Contains(search) || p.Name.Contains(search)))
                        .ToListAsync();
            
            Pager<Client> clientsPaginator = new Pager<Client>
                (
                    clients,
                    perPage, 
                    page
                );

            return Ok(clientsPaginator);
        }

        /**<summary>
         * Function that returns a client with your Id, and OrganizationId
         * </summary>
         * <param name="id"> clientId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Client>> GetClient([FromQuery][BindRequired] string id)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            var client = await _context.Clients.FindAsync(id, idOrg);
            if (client == null)
            {
                throw new ClientArgumentException("Client not found.");
            }

            return Ok(client);
        }

        /**<summary> 
         * Function that returns all valid clients from a organization
         * </summary>
         * <param name="clientId"> clientId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getClientAddressByClientId")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Client>>> GetClientAddressByClientId([FromQuery][BindRequired] string clientId)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var idOrg = user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            return Ok(await _context.ClientsAddresses
                .Where(p => p.OrganizationId == idOrg &&
                            p.ClientId == clientId &&
                            p.DeletedAt == null)
                .ToListAsync());
        }

        /**<summary>
         * Function that returns a client with your Id, and OrganizationId
         * </summary>
         * <param name="clientId"> clientId </param>
         * <param name="clientAddressId"> clientAddressId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getClientAddress")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Client>> GetClientAddress([FromQuery][BindRequired] string clientId, [FromQuery][BindRequired] string clientAddressId)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            var clientAddress = await _context.ClientsAddresses.FindAsync(clientAddressId, clientId, idOrg);
            if (clientAddress == null)
            {
                throw new ClientArgumentException("ClientAddress not found.");
            }

            if (clientAddress.DeletedAt != null)
            {
                throw new ClientArgumentException("ClientAddress is disable.");
            }

            return Ok(clientAddress);
        }

        /**<summary>
         * Add new client
         * </summary>
         */
        [HttpPost("addClientList")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Client>> AddClientList(CLientAndClientAddress clientAndClientAddress)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = (int)user.OrganizationId;

            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new LotArgumentException("Organization Not Found!");
            }

            Client client = clientAndClientAddress.client;
            client.OrganizationId = idOrg;
            List<ClientsAddress> clientAddress = clientAndClientAddress.clientsAddressesList;

            try
            {
                ClientRequestValidator.Validate(client);

                var result = await _context.Clients.FindAsync(client.Id, client.OrganizationId);

                if (result != null)
                {
                    throw new ClientArgumentException("Client already exist!");
                }

                _context.Clients.Add(client);
                await AddClientAddressList(client, clientAddress);

                await _context.SaveChangesAsync();

                return Ok(await _context.Clients.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Update client
         * </summary>
         * <param name="client"> Client data to update </param>
         * <param name="clientAddressList"> ClientsAdderss data to update</param>
         */
        [HttpPut("updateClientList")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Client>> UpdateClientList(CLientAndClientAddress clientAndClientAddress)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = (int)user.OrganizationId;

            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new LotArgumentException("Organization Not Found!");
            }

            Client client = clientAndClientAddress.client;
            client.OrganizationId = idOrg;
            List<ClientsAddress> clientAddress = clientAndClientAddress.clientsAddressesList;

            try
            {
                var result = await _context.Clients.FindAsync(client.Id, client.OrganizationId);

                if (result == null)
                {
                    throw new ClientArgumentException("Client not found!");
                }

                result.Email = client.Email;
                result.Name = client.Name;

                await RemoveAllClientAddress(client);
                await AddClientAddressList(client, clientAddress);

                await _context.SaveChangesAsync();

                return Ok(await _context.Clients.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Add new client
         * </summary>
         * <param name="client"> Client data to add </param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Client>> AddClient(Client client)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }
            
            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            try
            {
                ClientRequestValidator.Validate(client);

                var result = await _context.Clients.FindAsync(client.Id, client.OrganizationId);

                if (result != null)
                {
                    throw new ClientArgumentException("Client not found!");
                }

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return Ok(await _context.Clients.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Update client
         * </summary>
         * <param name="client"> Client data to update </param>
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Client>> UpdateClient(Client client)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            try
            {
                ClientRequestValidator.Validate(client);

                var result = await _context.Clients.FindAsync(client.Id, client.OrganizationId);

                if (result == null)
                {
                    throw new ClientArgumentException("Client not found!");
                }

                result.Email = client.Email;
                result.Name = client.Name;

                await _context.SaveChangesAsync();

                return Ok(await _context.Clients.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Add new client address
         * </summary>
         * <param name="clientAddress"> Client address to add </param>
         */
        [HttpPost("addClientAddress")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<ClientsAddress>>> AddClientAddress(ClientsAddress clientAddress)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            try
            {
                ClientAddressRequestValidator.Validate(clientAddress);

                var client = await _context.Clients.FindAsync(clientAddress.ClientId, clientAddress.OrganizationId);

                if (client == null)
                {
                    throw new ClientArgumentException("Client not found!");
                }

                var result = await _context.ClientsAddresses.FindAsync(clientAddress.Id, clientAddress.ClientId, clientAddress.OrganizationId);

                if (result != null && result.DeletedAt == null)
                {
                    throw new ClientArgumentException("This clientAddrress already existe!");
                }

                if (result != null &&
                    result.Id == clientAddress.Id &&
                    result.OrganizationId == clientAddress.OrganizationId &&
                    result.DeletedAt != null)
                {
                    result.DeletedAt = null;
                    await _context.SaveChangesAsync();
                } else if (result == null)
                {
                    _context.ClientsAddresses.Add(clientAddress);
                    await _context.SaveChangesAsync();
                }

                return Ok(await _context.ClientsAddresses
                    .Where(p => p.ClientId == client.Id &&
                                p.OrganizationId == client.OrganizationId &&
                                p.DeletedAt == null)
                    .ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Remove all clientAddress
         * </summary>
         * <param name="clientAddressId"> Client address to add </param>
         * <param name="clientId"> clientId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpDelete("removeClientAddress")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<ClientsAddress>>> RemoveClientAddress([FromQuery][BindRequired] string clientId, [FromQuery][BindRequired] string clientAddressId)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                var client = await _context.Clients.FindAsync(clientId, idOrg);

                if (client == null)
                {
                    throw new ClientArgumentException("Client not found!");
                }

                var clientAddress = await _context.ClientsAddresses.FindAsync(clientAddressId, clientId, idOrg);

                if (clientAddress == null)
                {
                    throw new ClientArgumentException("ClientAddress not found!");
                }

                if (clientAddress.DeletedAt != null)
                {
                    throw new ClientArgumentException("This ClientAddress is already removed.");
                }

                clientAddress.DeletedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(await _context.ClientsAddresses
                .Where(p => p.ClientId == client.Id &&
                            p.OrganizationId == client.OrganizationId &&
                            p.DeletedAt == null)
                .ToListAsync());
                              
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Add new list client address
         * </summary>
         * <param name="clientAddress"> Client address to add </param>
         * <param name="clientId"> clientId </param>
         */
        private async Task AddClientAddressList(Client client, List<ClientsAddress> clientAddressList)
        {
            try
            {
                if (client == null)
                {
                    throw new ClientArgumentException("Client not found!");
                }
 
                foreach (ClientsAddress clientAddress in clientAddressList)
                {
                    ClientAddressRequestValidator.Validate(clientAddress);

                    if (client.Id != clientAddress.ClientId && client.OrganizationId == clientAddress.OrganizationId)
                    {
                        throw new ClientArgumentException("This clientId/organizationId dont match with clientId/organizationId from ClientAddress!!");
                    }

                    var result = await _context.ClientsAddresses.FindAsync(clientAddress.Id, clientAddress.ClientId, clientAddress.OrganizationId);

                    if (result != null && result.DeletedAt != null)
                    {
                        result.DeletedAt = null;
                        result.Address = clientAddress.Address;
                        result.Zipcode = clientAddress.Zipcode;
                    } else if (result != null && result.DeletedAt == null)
                    {
                        result.Address = clientAddress.Address;
                        result.Zipcode = clientAddress.Zipcode;
                    } else
                    {
                        _context.ClientsAddresses.Add(clientAddress);
                    }
                }
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Remove all clientAddress
         * </summary>
         * <param name="client"> clientId </param>
         */
        private async Task RemoveAllClientAddress(Client client)
        {
            try
            {
                if (client == null)
                {
                    throw new ClientArgumentException("Client not found!");
                }

                var clientAddressList = await _context.ClientsAddresses
                    .Where(p => p.ClientId == client.Id &&
                                p.OrganizationId == client.OrganizationId)
                    .ToListAsync();

                foreach(var clientAddress in clientAddressList)
                {
                    clientAddress.DeletedAt = DateTime.Now;
                }
                
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary> 
         * Function that remove an existing client with your Id adn OrganizationId
         * <summary>
         * <param name="id"> clientId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpDelete]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Client>>> RemoveClient([FromQuery][BindRequired] string id)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                var result = await _context.Clients.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new ClientArgumentException("Client not found.");
                }

                var clientAddressList = await _context.ClientsAddresses
                    .Where( P => P.OrganizationId == idOrg && 
                                 P.ClientId == id)
                    .ToListAsync();  

                foreach(var clientAddress in clientAddressList)
                {
                    clientAddress.DeletedAt = DateTime.Now;
                }


                result.DeletedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(await _context.Clients.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new ClientArgumentException("You cant delete this Client");
            }
        } 
    }
}
