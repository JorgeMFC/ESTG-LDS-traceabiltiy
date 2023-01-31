import { Pagination } from "./Pagination";

export interface SizePagination extends Pagination {
  results: [
    {
      id: string;
      organizationId: number;
      size1: string;
    }
  ];
}

export type Size = {
  id: string;
  organizationId: number;
  size1: string;
};
