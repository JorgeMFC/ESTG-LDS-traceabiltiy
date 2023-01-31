import { Button } from "flowbite-react"

import { Form, Formik, FormikProps } from "formik"
import { useRef } from "react"
import toast from "react-hot-toast"
import { useMutation } from "react-query"
import MyCustomDialog from "../../../../components/MyCustomDialog"
import MyInput from "../../../../components/controls/MyInput"
import MySelect from "../../../../components/controls/MySelect"
import { DialogSizes } from "../../../../models/DialogSizes"
import { ComponentType } from "../../../../models/api/ComponentType"
import { Provider } from "../../../../models/api/Provider"
import { addComponentFormSchema } from "../../../../schemas/componentFormSchema"
import { addComponent } from "../../../../services/api/componentsApi"
import { queryClient } from "../../../../services/queryClient"

interface ComponentsAddProps {
	open: boolean
	handleVisibility: (visibility: boolean) => void
	componentTypes: ComponentType[]
	providers: Provider[]
}

interface FormValues {
	reference: string
	componentType: string
	provider: string
}

export default function ComponentsAdd(props: ComponentsAddProps) {
	const formRef = useRef<FormikProps<FormValues>>(null)

	const addComponentMutation = useMutation(addComponent, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("components-page-components")
			queryClient.invalidateQueries("components-page-componentTypes")
			queryClient.invalidateQueries("componentsByType-page-components")
			toast.success("Component inserted successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const onSubmit = (values: FormValues, actions: any) => {
		props.handleVisibility(false)
		actions.resetForm()
		addComponentMutation.mutate({
			id: values.reference,
			organizationId: 1,
			componentsTypeId: values.componentType,
			providerId: values.provider === "" ? undefined : values.provider,
		})
	}

	const handleSubmit = () => {
		if (formRef.current) {
			formRef.current.handleSubmit()
		}
	}

	const componentTypesId: string[] = []
	props.componentTypes.forEach((elem) => {
		componentTypesId.push(elem.componentsType.id!!.toString())
	})

	const providersId: string[] = []
	props.providers.forEach((elem) => {
		providersId.push(elem.id)
	})

	return (
		<MyCustomDialog
			title="Add component"
			open={props.open}
			handleVisibility={props.handleVisibility}
			content={
				<Formik
					innerRef={formRef}
					initialValues={{ reference: "", componentType: "", provider: "" }}
					validationSchema={() => addComponentFormSchema(componentTypesId, providersId)}
					onSubmit={onSubmit}
				>
					<Form>
						<div className="px-5">
							<div className="grid gap-10">
								<div className="grid gap-4">
									<MySelect label="Component Type" name="componentType">
										<option disabled value="">
											Please select a component type
										</option>
										{props.componentTypes.map((option) => (
											<option
												key={option.componentsType.id}
												value={option.componentsType.id!!.toString()}
											>
												{option.componentsType.componentType}
											</option>
										))}
									</MySelect>

									<MyInput label="Reference" name="reference" type="text" placeholder="Reference" />

									<MySelect label="Provider" name="provider">
										<option disabled value="">
											Please select a provider
										</option>
										{props.providers.map((option) => (
											<option key={option.id} value={option.id}>
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
	)
}
