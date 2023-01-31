import { useField } from "formik";

interface MySelectProps {
  label?: string;
  name: string;
  getFieldValue?: (value: string) => void;
  children: React.ReactNode;
}

const MySelect = ({ label, getFieldValue, ...props }: MySelectProps) => {
  const [field, meta] = useField(props);

  return (
    <div>
      <label className="block text-sm font-medium text-gray-700">{label}</label>
      <select
        {...field}
        {...props}
        onChange={(e) => {
          if (getFieldValue) getFieldValue(e.target.value);
          field.onChange(e);
        }}
        className={
          meta.touched && meta.error
            ? "transition duration-500 block w-full rounded-md border-red-400 focus:border-red-500 focus:ring-red-500 sm:text-sm"
            : "transition duration-500 block w-full rounded-md border-gray-300 focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
        }
      />
      {meta.touched && meta.error && (
        <p className="block text-xs font-medium text-red-500">{meta.error}</p>
      )}
    </div>
  );
};

export default MySelect;
