import * as yup from "yup"

export const addComponentFormSchema = (componentTypesId: string[], providersId: string[]) => {
	return yup.object().shape({
		reference: yup
			.string()
			.min(3, "Reference must be at least 3 characters long")
			.required("Required"),
		componentType: yup
			.string()
			.oneOf(componentTypesId, "Invalid Component Type")
			.required("Required"),
		provider: yup.string().oneOf(providersId, "Invalid Component Type"),
	})
}

export const editComponentFormSchema = (componentByTypeId: string[]) => {
	return yup.object().shape({
		reference: yup.string().oneOf(componentByTypeId, "Invalid Reference").required("Required"),
	})
}
