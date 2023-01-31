import { StagesModel } from "../../models/api/StagesModel"
import { StagesModelStage } from "../../models/api/StagesModelStage"
import { MyAPI } from "../MyAPI"
import {
	addStagesModelStage,
	removeStagesModelStage,
	updateStagesModelStage,
} from "./stagesModelStageApi"

interface StagesAux {
	id: string
	position: number
}

interface StagesModelAddEdit {
	stagesModel: StagesModel
	stages: Array<StagesAux>
}

export const getStagesModel = async (
	page: number = 1,
	perPage: number = 10,
	search: string = ""
) => {
	const response = await MyAPI().get(`StagesModel?page=${page}&perPage=${perPage}&search=${search}`)
	return response.data
}

export const addStagesModel = async (props: StagesModelAddEdit) => {
	const stagesModelResponse = await MyAPI().post("/StagesModel", props.stagesModel)
	props.stages.forEach(async (stage) => {
		const stagesModelStage: StagesModelStage = {
			organizationId: 1,
			position: stage.position,
			stagesId: stage.id,
			stagesModelId: props.stagesModel.id,
		}

		await addStagesModelStage(stagesModelStage)
	})

	return stagesModelResponse
}

export const updateStagesModel = async (props: StagesModelAddEdit) => {
	await MyAPI().put("/StagesModel", props.stagesModel)

	const stagesModelStagesList: Array<StagesModelStage> = []
	props.stages.forEach((stage) => {
		const stagesModelStage: StagesModelStage = {
			organizationId: 1,
			position: stage.position,
			stagesId: stage.id,
			stagesModelId: props.stagesModel.id,
		}
		stagesModelStagesList.push(stagesModelStage)
	})

	return await updateStagesModelStage(stagesModelStagesList)
}

export const removeStagesModel = async (stageModelId: string) => {
	await removeStagesModelStage(stageModelId)

	const result = await MyAPI().delete(`/StagesModel?id=${stageModelId}`)

	return result.data
}
