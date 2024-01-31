import { FC } from "react";
import classNames from "classnames";
import { UserProps } from "../UserCard/User.component";
import User from "../UserCard";

import './UserList.styles.scss'

type UserlistProps = {
  title: string;
  wrap?: boolean;
  className?: string;
  usersData?: Omit<UserProps, "className">[];
};

const UserList: FC<UserlistProps> = ({
  className: classes,
  title,
  usersData,
  wrap = true,
}) => {
  const className = classNames("user-list", classes);
  return (
    <div className={className}>
      <h2 className="user-list__title">{title}</h2>
      <div
        className={`user-list__users user-list__users${
          wrap ? "--wrap" : "--no-wrap"
        }`}
      >
        {usersData &&
          usersData.map((user, index) => {
            return <User className="user-list__users__user" {...user} />;
          })}
      </div>
    </div>
  );
};

export default UserList;
