import { Button } from "flowbite-react"
import { useRef, useState } from "react"

import { Form, Formik, FormikProps } from "formik"
import toast from "react-hot-toast"
import { useMutation, useQuery } from "react-query"
import DialogRemove from "../../../../components/DialogRemove"
import MyCustomDialog from "../../../../components/MyCustomDialog"
import MyInput from "../../../../components/controls/MyInput"
import MySelect from "../../../../components/controls/MySelect"
import { DialogSizes } from "../../../../models/DialogSizes"
import { Component } from "../../../../models/api/Component"
import { ComponentType } from "../../../../models/api/ComponentType"
import { Provider } from "../../../../models/api/Provider"
import { editComponentFormSchema } from "../../../../schemas/componentFormSchema"
import {
	deleteComponent,
	getComponentsByType,
	updateComponent,
} from "../../../../services/api/componentsApi"
import { queryClient } from "../../../../services/queryClient"

interface ComponentsEditProps {
	open: boolean
	handleVisibility: (visibility: boolean) => void
	componentType: ComponentType
	providers: Provider[]
}

interface FormValues {
	reference: string
	componentType: string
	provider: string
}

export default function ComponentsEdit(props: ComponentsEditProps) {
	const [openRemove, setOpenRemove] = useState(false)
	const handleVisibilityRemove = (visibility: boolean) => {
		setOpenRemove(visibility)
	}

	const {
		isLoading: isLoadingComponentsByType,
		isError: isErrorComponentsByType,
		error: errorComponentsByType,
		data: componentsByType,
	} = useQuery<Component[], Error>(["componentsByType-page-components", props.componentType], () =>
		getComponentsByType({
			componentTypeId: props.componentType.componentsType.id!!,
		})
	)

	const componentByTypeId: string[] = []
	componentsByType?.forEach((elem) => {
		componentByTypeId.push(elem.id.toString())
	})

	const formRef = useRef<FormikProps<FormValues>>(null)

	const updateComponentMutation = useMutation(updateComponent, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("componentsByType-page-components")
			toast.success("Component updated successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const deleteComponentMutation = useMutation(deleteComponent, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("components-page-components")
			queryClient.invalidateQueries("componentsByType-page-components")
			toast.success("Component deleted successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const onSubmit = (values: FormValues, actions: any) => {
		console.log(values)
		actions.resetForm()

		if (openRemove) {
			deleteComponentMutation.mutate({
				id: values.reference,
			})
			handleVisibilityRemove(false)
		} else {
			updateComponentMutation.mutate({
				id: values.reference,
				organizationId: 1,
				componentsTypeId: props.componentType.componentsType.id!!.toString(),
				providerId: values.provider === "" ? undefined : values.provider,
			})
			props.handleVisibility(false)
		}
	}

	const handleSubmit = () => {
		if (formRef.current) {
			//If there are any errors, the delete modal needs to be hidden to show the errors, since the delete modal shows
			//on top of the edit modal
			if (!formRef.current.isValid) handleVisibilityRemove(false)
			formRef.current.handleSubmit()
		}
	}

	const [selectedReferenceId, setSelectedReferenceId] = useState<string>()

	const selectedReference = componentsByType?.filter(
		(elem) => elem.id === selectedReferenceId
	)[0] ?? { providerId: "", id: "" }
	if (selectedReference.providerId === null) selectedReference.providerId = ""

	return (
		<>
			<MyCustomDialog
				title="Edit component"
				open={props.open}
				handleVisibility={props.handleVisibility}
				content={
					<Formik
						innerRef={formRef}
						initialValues={{
							reference: selectedReference.id,
							provider: selectedReference.providerId ?? "",
							componentType: props.componentType.componentsType.componentType,
						}}
						validationSchema={() => editComponentFormSchema(componentByTypeId)}
						onSubmit={onSubmit}
						enableReinitialize
					>
						<Form>
							<div className="px-5">
								<div className="grid gap-10">
									<div className="grid gap-4">
										<MyInput
											label="Component Type"
											name="componentType"
											type="text"
											placeholder="Component Type"
											disabled
										/>

										<MySelect
											label="Reference"
											name="reference"
											getFieldValue={setSelectedReferenceId}
										>
											<option disabled value="">
												Please select a reference
											</option>
											{componentsByType?.map((option) => (
												<option key={option.id} value={option.id}>
													{option.id}
												</option>
											))}
										</MySelect>

										<MySelect label="Provider" name="provider">
											<option disabled value="">
												Please select a provider
											</option>
											{componentsByType &&
												componentsByType.length > 0 &&
												props.providers.map((option) => (
													<option
														key={option.id}
														value={option.id}
														disabled={
															selectedReference && selectedReference.providerId === option.id
														}
													>
														{option.name}
													</option>
												))}
										</MySelect>
									</div>
								</div>
							</div>
						</Form>
					</Formik>
				}
				actions={
					<div>
						<div className="flex flex-row-reverse gap-2">
							<Button color="dark" size="md" onClick={() => handleVisibilityRemove(true)}>
								Remove
							</Button>
							<Button color="dark" size="md" onClick={handleSubmit}>
								Save
							</Button>
							<Button color="dark" size="md" onClick={() => props.handleVisibility(false)}>
								Close
							</Button>
						</div>
					</div>
				}
				size={DialogSizes.SMALL}
			/>
			<DialogRemove
				message={`Are you sure you want to remove the component with the reference ${selectedReference.id} of type ${props.componentType.componentsType.componentType} from the list?`}
				open={openRemove}
				actionButtonYes={() => {
					handleSubmit()
					//handleVisibilityRemove(false)
				}}
				handleVisibility={handleVisibilityRemove}
				actionButtonCancel={() => handleVisibilityRemove(false)}
			/>
		</>
	)
}
