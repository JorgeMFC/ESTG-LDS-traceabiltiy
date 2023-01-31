import { useField } from "formik";

interface MyInputProps {
  label: string;
  name: string;
  type: string;
  placeholder: string;
  disabled?: boolean;
}

const MyInput = ({ label, ...props }: MyInputProps) => {
  const [field, meta] = useField(props);

  return (
    <div>
      <label className="block text-sm font-medium text-gray-700">{label}</label>
      <input
        {...field}
        {...props}
        className={
          meta.touched && meta.error
            ? "transition duration-500 disabled:bg-slate-50 disabled:text-slate-500 disabled:border-slate-200 disabled:shadow-none block w-full rounded-md border-red-400 focus:border-red-500 focus:ring-red-500 sm:text-sm"
            : "transition duration-500 disabled:bg-slate-50 disabled:text-slate-500 disabled:border-slate-200 disabled:shadow-none block w-full rounded-md border-gray-300 focus:border-blue-500 focus:ring-blue-500 sm:text-sm"
        }
      />
      {meta.touched && meta.error && (
        <p className="block text-xs font-medium text-red-500">{meta.error}</p>
      )}
    </div>
  );
};

export default MyInput;
