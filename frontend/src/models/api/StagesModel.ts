import { Pagination } from "./Pagination"

export interface StagesModel {
	id: string
	organizationId: number
	stagesModelName: string
}

export interface StagesModelPaginated extends Pagination {
	results: Array<StagesModel>
}
