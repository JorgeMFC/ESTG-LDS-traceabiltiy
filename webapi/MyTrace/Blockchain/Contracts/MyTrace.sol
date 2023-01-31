// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;

import "./OpenZeppelin/contracts@4.8.0/token/ERC721/ERC721.sol";
import "./OpenZeppelin/contracts@4.8.0/token/ERC721/extensions/ERC721URIStorage.sol";
import "./OpenZeppelin/contracts@4.8.0/security/Pausable.sol";
import "./OpenZeppelin/contracts@4.8.0/access/Ownable.sol";
import "./OpenZeppelin/contracts@4.8.0/utils/Counters.sol";

contract MyTrace is ERC721, ERC721URIStorage, Pausable, Ownable {
    using Counters for Counters.Counter;

    Counters.Counter private _tokenIdCounter;

    constructor() ERC721("MyTrace", "MT") {}

    function _baseURI() internal pure override returns (string memory) {
        return "https://ipfs.io/ipfs/";
    }

    function pause() public onlyOwner {
        _pause();
    }

    function unpause() public onlyOwner {
        _unpause();
    }

    function safeMint(address to, string memory uri) public onlyOwner {
        uint256 tokenId = _tokenIdCounter.current();
        _tokenIdCounter.increment();
        _safeMint(to, tokenId);
        _setTokenURI(tokenId, uri);
    }

    function _beforeTokenTransfer(address from, address to, uint256 tokenId, uint256 batchSize)
        internal
        whenNotPaused
        override
    {
        super._beforeTokenTransfer(from, to, tokenId, batchSize);
    }

    // The following functions are overrides required by Solidity.

    function _burn(uint256 tokenId) internal override(ERC721, ERC721URIStorage) {
        super._burn(tokenId);
    }

    function mint(address recipient, string memory uri) public returns (uint256) {
        uint256 newId = _tokenIdCounter.current();
        _mint(recipient, newId);
        _setTokenURI(newId, uri);
        _tokenIdCounter.increment();
        return newId;
    }


    function tokenURI(uint256 tokenId)
        public
        view
        override(ERC721, ERC721URIStorage)
        returns (string memory)
    {
        return super.tokenURI(tokenId);
    }
}
