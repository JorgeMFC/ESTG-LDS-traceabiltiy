import { Pagination } from "./Pagination";

export interface ProviderPagination extends Pagination {
  results: [
    {
      id: string;
      organizationId: number;
      name: string;
      email: string;
      createdAt: string;
      deletedAt: string | null;
    }
  ];
}

export interface Provider {
  id: string;
  organizationId: number;
  name: string;
  email: string;
  createdAt: string | null;
  deletedAt: string | null;
}

export interface ProviderResult extends Pagination {
  results: Array<Provider>;
}
