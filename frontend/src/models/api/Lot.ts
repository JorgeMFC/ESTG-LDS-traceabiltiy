import { Pagination } from './Pagination';

export interface Lot {
  id: string;
  organizationId: number;
  modelId: string;
  modelColorId: string;
  modelSizeId: string;
  clientId: string;
  clientAddressId: string;
  organizationAddressId: string;
  deliveryDate: string | null;
  lotSize: number;
  hash: string;
  stagesModelId: string;
  canceledAt: string | null;
}

export interface LotNames {
  lot: Lot;
  nomeModelo: string;
  nomeEtapa: string;
  nomeCliente: string;
  nomeCor: string;
  nomeTamanho: number;
}

export interface LotPaginated extends Pagination {
  results: Array<LotNames>;
}
