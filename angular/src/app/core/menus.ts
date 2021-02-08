import { Menu } from '@delon/theme';

const MENUS: Menu[] = [
  {
    text: '主菜单',
    group: true,
    hideInBreadcrumb: true,
    children: [
      {
        text: '仪表盘',
        link: '/dashboard',
        icon: { type: 'icon', value: 'appstore' }
      }
    ]
  }
];

export default MENUS;
