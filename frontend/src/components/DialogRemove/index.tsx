import { Button } from "flowbite-react"
import { DialogSizes } from "../../models/DialogSizes"

import MyCustomDialog from "../MyCustomDialog"

interface ModelRemoveProps {
	open: boolean
	handleVisibility: (visibility: boolean) => void
	message: string
	actionButtonYes: () => void
	actionButtonCancel: () => void
	imageSrc?: string
}

export default function ModelRemove(props: ModelRemoveProps) {
	return (
		<MyCustomDialog
			size={DialogSizes.SMALL}
			open={props.open}
			handleVisibility={props.handleVisibility}
			content={
				<div className="grid place-items-center pt-2">
					{props.imageSrc && <img className="object-cover h-48 w-96" src={props.imageSrc} />}
					<div className="w-96">{props.message}</div>
					<div className="gap-2 flex pt-2">
						<Button color="dark" size="md" className="m-2" onClick={() => props.actionButtonYes()}>
							Yes
						</Button>
						<Button
							color="dark"
							size="md"
							className="m-2"
							onClick={() => props.actionButtonCancel()}
						>
							Cancel
						</Button>
					</div>
				</div>
			}
		/>
	)
}
