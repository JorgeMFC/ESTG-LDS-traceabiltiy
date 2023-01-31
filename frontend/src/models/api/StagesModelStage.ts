import { Pagination } from "./Pagination"

export interface StagesModelStage {
	stagesModelId: string
	stagesId: string
	organizationId: number
	position: number
}

export interface StagesModelStagePaginated extends Pagination {
	results: Array<StagesModelStage>
}
