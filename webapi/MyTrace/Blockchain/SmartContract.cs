using Contracts.Contracts.MyTrace;
using MyTrace.Models;
using MyTrace.Utils;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace MyTrace.Blockchain
{
    public class SmartContract
    {
        private readonly PinataEnv _pinataEnv;
        private readonly string _pathCache;
		private readonly string contractAddress;
		private readonly CryptocurrencyBrokerageEnv _cryptocurrencyBrokerageEnv;

        public SmartContract(
            CryptocurrencyBrokerageEnv cryptocurrencyBrokerageEnv,
            PinataEnv pinataEnv,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _pinataEnv = pinataEnv;
            _pathCache = webHostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "cache");
            _cryptocurrencyBrokerageEnv = cryptocurrencyBrokerageEnv;
			contractAddress = cryptocurrencyBrokerageEnv.ContractAddress;
        }

        private async Task<Web3> authenticate(CryptocurrencyBrokerageEnv _cryptocurrencyBrokerageEnv)
        {
			Web3 web3 = null;
            foreach (var network in _cryptocurrencyBrokerageEnv.Url)
            {
				try
				{
                    web3 = new Web3(network);
                    var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                    web3.TransactionManager.UseLegacyAsDefault = true;
                    if (latestBlockNumber != null)
                    {
                        break;
                    }
                }
				catch (Exception)
				{
				}
            }
            return web3;
        }

        private async Task<string?> publicNFT(JsonObject jsonObj)
        {
            if (!Directory.Exists(_pathCache))
            {
                Directory.CreateDirectory(_pathCache);
            }

            string filename = Cryptography.HashString(JsonFileUtils.JsonPrettify(jsonObj), "MyTrace") + ".json";
            string file = Path.Combine(_pathCache, filename);

            await JsonFileUtils.WriteObjectInFileAsync(file, jsonObj);

            string? uri = await Pinata.CreateNFT(file, _pinataEnv);

            JsonFileUtils.DeleteFile(file);

            return uri;
        }

        public async Task<string?> CreateNFTLot(Lot lot, Organization organization)
        {
            JsonObject jsonObj = new()
            {
                ["OrganizationWallet"] = organization.WalletAddress,
                ["Size"] = lot.LotSize,
                ["ModelId"] = lot.ModelId,
                ["ModelColorId"] = lot.ModelColorId,
                ["ModelSizeId"] = lot.ModelSizeId,
                ["LotSize"] = lot.LotSize,
            };

            return await publicNFT(jsonObj);
        }

        public async Task<string?> CreateNFTState(Lot lot, Stage stage)
        {
            JsonObject jsonObj = new()
            {
                ["LotHash"] = lot.Hash,
                ["StageName"] = stage.StageName,
                ["StageDescription"] = stage.StageDescription,
            };

            return await publicNFT(jsonObj);
        }

        public async Task<bool> validateHashTransaction(string transactionHash)
        {
            Web3 web3 = await authenticate(_cryptocurrencyBrokerageEnv);
            try
            {
                if (transactionHash == null)
                {
                    return false;
                }

                string abi = @"
				[
					{
						""inputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""constructor""
					},
					{
						""anonymous"": false,
						""inputs"": [
							{
								""indexed"": true,
								""internalType"": ""address"",
								""name"": ""owner"",
								""type"": ""address""
							},
							{
								""indexed"": true,
								""internalType"": ""address"",
								""name"": ""approved"",
								""type"": ""address""
							},
							{
								""indexed"": true,
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							}
						],
						""name"": ""Approval"",
						""type"": ""event""
					},
					{
						""anonymous"": false,
						""inputs"": [
							{
								""indexed"": true,
								""internalType"": ""address"",
								""name"": ""owner"",
								""type"": ""address""
							},
							{
								""indexed"": true,
								""internalType"": ""address"",
								""name"": ""operator"",
								""type"": ""address""
							},
							{
								""indexed"": false,
								""internalType"": ""bool"",
								""name"": ""approved"",
								""type"": ""bool""
							}
						],
						""name"": ""ApprovalForAll"",
						""type"": ""event""
					},
					{
						""anonymous"": false,
						""inputs"": [
							{
								""indexed"": true,
								""internalType"": ""address"",
								""name"": ""previousOwner"",
								""type"": ""address""
							},
							{
								""indexed"": true,
								""internalType"": ""address"",
								""name"": ""newOwner"",
								""type"": ""address""
							}
						],
						""name"": ""OwnershipTransferred"",
						""type"": ""event""
					},
					{
						""anonymous"": false,
						""inputs"": [
							{
								""indexed"": false,
								""internalType"": ""address"",
								""name"": ""account"",
								""type"": ""address""
							}
						],
						""name"": ""Paused"",
						""type"": ""event""
					},
					{
						""anonymous"": false,
						""inputs"": [
							{
								""indexed"": true,
								""internalType"": ""address"",
								""name"": ""from"",
								""type"": ""address""
							},
							{
								""indexed"": true,
								""internalType"": ""address"",
								""name"": ""to"",
								""type"": ""address""
							},
							{
								""indexed"": true,
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							}
						],
						""name"": ""Transfer"",
						""type"": ""event""
					},
					{
						""anonymous"": false,
						""inputs"": [
							{
								""indexed"": false,
								""internalType"": ""address"",
								""name"": ""account"",
								""type"": ""address""
							}
						],
						""name"": ""Unpaused"",
						""type"": ""event""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""to"",
								""type"": ""address""
							},
							{
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							}
						],
						""name"": ""approve"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""owner"",
								""type"": ""address""
							}
						],
						""name"": ""balanceOf"",
						""outputs"": [
							{
								""internalType"": ""uint256"",
								""name"": """",
								""type"": ""uint256""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							}
						],
						""name"": ""getApproved"",
						""outputs"": [
							{
								""internalType"": ""address"",
								""name"": """",
								""type"": ""address""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""owner"",
								""type"": ""address""
							},
							{
								""internalType"": ""address"",
								""name"": ""operator"",
								""type"": ""address""
							}
						],
						""name"": ""isApprovedForAll"",
						""outputs"": [
							{
								""internalType"": ""bool"",
								""name"": """",
								""type"": ""bool""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""recipient"",
								""type"": ""address""
							},
							{
								""internalType"": ""string"",
								""name"": ""uri"",
								""type"": ""string""
							}
						],
						""name"": ""mint"",
						""outputs"": [
							{
								""internalType"": ""uint256"",
								""name"": """",
								""type"": ""uint256""
							}
						],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [],
						""name"": ""name"",
						""outputs"": [
							{
								""internalType"": ""string"",
								""name"": """",
								""type"": ""string""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [],
						""name"": ""owner"",
						""outputs"": [
							{
								""internalType"": ""address"",
								""name"": """",
								""type"": ""address""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							}
						],
						""name"": ""ownerOf"",
						""outputs"": [
							{
								""internalType"": ""address"",
								""name"": """",
								""type"": ""address""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [],
						""name"": ""pause"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [],
						""name"": ""paused"",
						""outputs"": [
							{
								""internalType"": ""bool"",
								""name"": """",
								""type"": ""bool""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [],
						""name"": ""renounceOwnership"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""to"",
								""type"": ""address""
							},
							{
								""internalType"": ""string"",
								""name"": ""uri"",
								""type"": ""string""
							}
						],
						""name"": ""safeMint"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""from"",
								""type"": ""address""
							},
							{
								""internalType"": ""address"",
								""name"": ""to"",
								""type"": ""address""
							},
							{
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							}
						],
						""name"": ""safeTransferFrom"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""from"",
								""type"": ""address""
							},
							{
								""internalType"": ""address"",
								""name"": ""to"",
								""type"": ""address""
							},
							{
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							},
							{
								""internalType"": ""bytes"",
								""name"": ""data"",
								""type"": ""bytes""
							}
						],
						""name"": ""safeTransferFrom"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""operator"",
								""type"": ""address""
							},
							{
								""internalType"": ""bool"",
								""name"": ""approved"",
								""type"": ""bool""
							}
						],
						""name"": ""setApprovalForAll"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""bytes4"",
								""name"": ""interfaceId"",
								""type"": ""bytes4""
							}
						],
						""name"": ""supportsInterface"",
						""outputs"": [
							{
								""internalType"": ""bool"",
								""name"": """",
								""type"": ""bool""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [],
						""name"": ""symbol"",
						""outputs"": [
							{
								""internalType"": ""string"",
								""name"": """",
								""type"": ""string""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							}
						],
						""name"": ""tokenURI"",
						""outputs"": [
							{
								""internalType"": ""string"",
								""name"": """",
								""type"": ""string""
							}
						],
						""stateMutability"": ""view"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""from"",
								""type"": ""address""
							},
							{
								""internalType"": ""address"",
								""name"": ""to"",
								""type"": ""address""
							},
							{
								""internalType"": ""uint256"",
								""name"": ""tokenId"",
								""type"": ""uint256""
							}
						],
						""name"": ""transferFrom"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [
							{
								""internalType"": ""address"",
								""name"": ""newOwner"",
								""type"": ""address""
							}
						],
						""name"": ""transferOwnership"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					},
					{
						""inputs"": [],
						""name"": ""unpause"",
						""outputs"": [],
						""stateMutability"": ""nonpayable"",
						""type"": ""function""
					}
				]    
				";

                var contract = web3.Eth.GetContract(abi, "0xCF818141c99DBc1F1601ED7904Ac086d8425F41C");
                var ctorTransaction = await contract.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionHash);

                return ctorTransaction.To.Equals(contractAddress, StringComparison.CurrentCultureIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
