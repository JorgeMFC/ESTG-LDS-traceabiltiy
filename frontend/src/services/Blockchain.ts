import { ethers } from 'ethers';
import abi from './MyTraceContract.json';

export async function publicNFT(
  receiver: String,
  urlNft: String,
  onSuccess: (hash: string) => void,
  onError: (error: string) => void
) {
  let ethereum = (window as any).ethereum;

  if (!ethereum) {
    onError('No crypto wallet found. Please install it.');
    return;
  }
  const provider = new ethers.providers.Web3Provider(ethereum);
  const network = await provider.getNetwork();

  if (network.chainId != import.meta.env.VITE_CONTRACT_CHAIN_ID) {
    throw new Error(
      `Select in the MetaMask a ${
        import.meta.env.VITE_CONTRACT_NETWORK_NAME
      } network`
    );
  }

  const signer = provider.getSigner();
  const contract = new ethers.Contract(
    import.meta.env.VITE_CONTRACT_ADDRESS,
    abi,
    signer
  );

  await contract
    .mint(receiver, urlNft)
    .then((response: any) => {
      onSuccess(`${response['hash']}`);
    })
    .catch((err: Error) => {
      onError(err.message);
    });
}
