import * as yup from "yup"

export const addProductionProcessFormSchema = (stagesId: string[]) => {
	return yup.object().shape({
		stagesModelId: yup
			.string()
			.min(3, "Id must be at least 3 characters long")
			.required("Required"),
		stagesModelName: yup
			.string()
			.min(3, "stagesModelName must be at least 3 characters long")
			.required("Required"),
		stages: yup.array().of(
			yup.object().shape({
				stageName: yup.string().oneOf(stagesId, "Invalid Stage").required("Required"),
			})
		),
	})
}

export const editProductionProcessFormSchema = (stagesId: string[]) => {
	return yup.object().shape({
		stagesModelId: yup
			.string()
			.min(3, "Id must be at least 3 characters long")
			.required("Required"),
		stagesModelName: yup
			.string()
			.min(3, "stagesModelName must be at least 3 characters long")
			.required("Required"),
		stages: yup.array().of(
			yup.object().shape({
				stageId: yup.string().oneOf(stagesId, "Invalid Stage").required("Required"),
			})
		),
	})
}
