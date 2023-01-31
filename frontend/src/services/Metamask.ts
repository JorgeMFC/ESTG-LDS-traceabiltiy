import { getSignatureMessageRequest } from './api/authenticationApi';

import { ethers } from 'ethers';

function titleCase(string: String) {
  return string[0].toUpperCase() + string.slice(1).toLowerCase();
}

export const metaSignMessage = async () => {
  try {
    const ethereum = (window as any).ethereum;
    if (!ethereum) {
      throw new Error('No crypto wallet found. Please install it.');
    }

    const accounts = await ethereum.request({
      method: 'eth_requestAccounts',
    });

    const provider = new ethers.providers.Web3Provider(ethereum);
    const network = await provider.getNetwork();

    if (network.chainId != import.meta.env.VITE_CONTRACT_CHAIN_ID) {
      throw new Error(
        `Select in the MetaMask a ${
          import.meta.env.VITE_CONTRACT_NETWORK_NAME
        } network`
      );
    }

    if (!accounts || !accounts.length) {
      throw new Error('Wallet not found/allowed!');
    }

    const message = await getSignatureMessageRequest({
      wallet: accounts[0],
    });

    const signer = provider.getSigner();
    const signature = await signer.signMessage(message);
    const address = await signer.getAddress();

    return {
      wallet: address,
      signature: signature,
    };
  } catch (err) {
    console.log(err);
    throw new Error(titleCase((err as Error).message.split('(')[0]));
  }
};
