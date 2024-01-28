import { FC } from "react";
import classNames from "classnames";

import "./GenreCard.styles.scss";

export type GenreProps = {
  name: String;
  className?: String;
};

const GenreCard: FC<GenreProps> = ({ className: classes, name }) => {
  const className = classNames("genre", classes);
  return (
    <div className={className}>
      <div className="genre__name">{name}</div>
    </div>
  );
};

export default GenreCard;
